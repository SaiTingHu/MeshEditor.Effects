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
        /// 碎片行为类型
        /// </summary>
        public string TransformerType = "MeshEditor.Effects.TransformerBehaviour";

        private bool _isTransformer = false;
        private Queue<TransformerBehaviour> _transformerPool = new Queue<TransformerBehaviour>();
        private int _timer = 0;
        private Type _transformerType;
        private Mesh _targetMesh;
        
        protected override void Awake()
        {
            base.Awake();

            _transformerType = Toolkit.GetTypeInRunTimeAssemblies(TransformerType);

            _targetMesh = new Mesh();
            _targetMesh.vertices = TargetMesh.vertices;
            _targetMesh.normals = TargetMesh.normals;
            _targetMesh.uv = TargetMesh.uv;
            _targetMesh.triangles = TargetMesh.triangles;
            _targetMesh.RecalculateBounds();
            _targetMesh.RecalculateNormals();
        }
        
        protected override void BeginEffect(MeshData meshData)
        {
            if (DisassembleRate < 0)
            {
                DisassembleRate = 1;
            }

            if (_isTransformer)
            {
                _isTransformer = false;
                _meshFilter.mesh = _mesh;
            }
        }

        protected override void UpdateEffect(MeshData meshData)
        {
            if (!_isTransformer)
            {
                _timer = DisassembleRate;
                while (_timer > 0)
                {
                    _timer -= 1;
                    Disassemble(meshData);
                }
            }
            else
            {

            }
        }

        protected override void EndEffect(MeshData meshData)
        {
            
        }

        //分解
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
                _isTransformer = true;
                _timer = 0;
            }
        }

        //生成碎片行为对象
        private void GenerateTransformer(Triangle triangle)
        {
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
            transformerBehaviour.Activate(this, triangle, _materials, 1, 10);
        }
    }
}