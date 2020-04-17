using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格变形特效
    /// </summary>
    [DisallowMultipleComponent]
    public class MeshTransformer : MeshEffectsBase
    {
        /// <summary>
        /// 目标网格
        /// </summary>
        public Mesh TargetMesh;
        /// <summary>
        /// 目标材质
        /// </summary>
        public Material[] TargetMaterials;
        /// <summary>
        /// 分解速率
        /// </summary>
        public int DisassembleRate = 5;
        /// <summary>
        /// 组装速率
        /// </summary>
        public int AssembleRate = 5;
        /// <summary>
        /// 碎片行为类型
        /// </summary>
        public string TransformerType = "MeshEditor.Effects.TransformerBehaviour";

        private TransformerState _state = TransformerState.None;
        private Queue<TransformerBehaviour> _transformerPool = new Queue<TransformerBehaviour>();
        private int _timer = 0;
        private Type _transformerType;

        private Mesh _targetMesh;
        private MeshData _targetData;
        private List<Triangle> _targetTrianglesPool = new List<Triangle>();

        public TransformerState State
        {
            get
            {
                return _state;
            }
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    switch (_state)
                    {
                        case TransformerState.None:
                            _meshFilter.mesh = _mesh;
                            break;
                        case TransformerState.Disassemble:
                            _meshFilter.mesh = _mesh;
                            _targetTrianglesPool.AddRange(_targetData.Triangles);
                            _targetData.ClearTriangles();
                            _targetData.ApplyData();
                            break;
                        case TransformerState.Assemble:
                            _meshFilter.mesh = _targetMesh;
                            break;
                    }
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _transformerType = Toolkit.GetTypeInRunTimeAssemblies(TransformerType);

            CreateTargetMesh(TargetMesh);

            State = TransformerState.Disassemble;
        }
        
        protected override void BeginEffect(MeshData meshData)
        {
            if (DisassembleRate < 0)
            {
                DisassembleRate = 1;
            }

            State = TransformerState.Disassemble;
        }

        protected override void UpdateEffect(MeshData meshData)
        {
            switch (State)
            {
                case TransformerState.None:
                    Stop();
                    break;
                case TransformerState.Disassemble:
                    _timer = DisassembleRate;
                    while (_timer > 0)
                    {
                        _timer -= 1;
                        Disassemble(meshData);
                    }
                    break;
                case TransformerState.Assemble:
                    _timer = AssembleRate;
                    while (_timer > 0)
                    {
                        _timer -= 1;
                        Assemble();
                    }
                    break;
            }
        }

        protected override void EndEffect(MeshData meshData)
        {
            
        }

        private void CreateTargetMesh(Mesh targetMesh)
        {
            _targetMesh = new Mesh();
            _targetMesh.vertices = targetMesh.vertices;
            _targetMesh.normals = targetMesh.normals;
            _targetMesh.uv = targetMesh.uv;
            _targetMesh.triangles = targetMesh.triangles;
            _targetData = new MeshData(_targetMesh);
        }

        private void Disassemble(MeshData meshData)
        {
            if (meshData.Triangles.Count > 0)
            {
                Triangle triangle = meshData.Triangles[0];
                meshData.RemoveAtTriangle(0);
                GenerateTransformer(triangle);
            }
            else
            {
                State = TransformerState.Assemble;
                _timer = 0;
            }
        }

        private void Assemble()
        {
            if (_targetTrianglesPool.Count > 0)
            {
                Triangle triangle = _targetTrianglesPool[0];
                _targetTrianglesPool.RemoveAt(0);

                TransformerBehaviour transformerBehaviour;
                if (_transformerPool.Count > 0)
                {
                    transformerBehaviour = _transformerPool.Dequeue();
                }
                else
                {
                    GameObject obj = new GameObject("Fragment");
                    obj.transform.SetParent(transform);
                    transformerBehaviour = obj.AddComponent(_transformerType) as TransformerBehaviour;
                }

                transformerBehaviour.gameObject.SetActive(true);
                transformerBehaviour.Activate(this, triangle, TargetMaterials, 1, 10);
            }
            else
            {
                State = TransformerState.None;
                _timer = 0;
            }
        }

        private void GenerateTransformer(Triangle triangle)
        {
            GameObject obj = new GameObject("Fragment");
            obj.transform.SetParent(transform);
            TransformerBehaviour transformerBehaviour = obj.AddComponent(_transformerType) as TransformerBehaviour;

            transformerBehaviour.gameObject.SetActive(true);
            transformerBehaviour.Activate(this, triangle, _materials, 1, 10);
            _transformerPool.Enqueue(transformerBehaviour);
        }
        
        public enum TransformerState
        {
            /// <summary>
            /// 空闲
            /// </summary>
            None,
            /// <summary>
            /// 分解
            /// </summary>
            Disassemble,
            /// <summary>
            /// 组装
            /// </summary>
            Assemble
        }
    }
}