using System;
using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格波动特效
    /// </summary>
    [DisallowMultipleComponent]
    public class MeshWave : MeshEffectsBase
    {
        /// <summary>
        /// 波动方向
        /// </summary>
        public WaveDirection Direction = WaveDirection.Y;
        /// <summary>
        /// 波动力度
        /// </summary>
        public float WavePower = 1;
        /// <summary>
        /// 波动速度
        /// </summary>
        public float WaveSpeed = 1;
        
        private Func<Vector3, Vector3> _applyWave;
        private float _weight;
        private float _2PI;
        private bool _isGenerate = false;
        private Vector3[] _positions;

        protected override void Awake()
        {
            base.Awake();

            _weight = 0;
            _2PI = Mathf.PI * 2;
        }

        protected override void BeginEffect(MeshData meshData)
        {
            switch (Direction)
            {
                case WaveDirection.X:
                    _applyWave = ApplyWaveX;
                    break;
                case WaveDirection.Y:
                    _applyWave = ApplyWaveY;
                    break;
                case WaveDirection.Z:
                    _applyWave = ApplyWaveZ;
                    break;
            }

            if (!_isGenerate)
            {
                _isGenerate = true;
                _positions = new Vector3[meshData.Vertices.Count];
                for (int i = 0; i < meshData.Vertices.Count; i++)
                {
                    _positions[i] = meshData.Vertices[i].Position;
                }
            }
        }

        protected override void UpdateEffect(MeshData meshData)
        {
            UpdateWeight();

            for (int i = 0; i < meshData.Vertices.Count; i++)
            {
                Vertex vertex = meshData.Vertices[i];
                vertex.Position = _applyWave(_positions[i]);
            }
        }

        protected override void EndEffect(MeshData meshData)
        {
            
        }

        private void UpdateWeight()
        {
            if (_weight < _2PI)
            {
                _weight += Time.deltaTime * WaveSpeed;
            }
            else
            {
                _weight = 0;
            }
        }

        private Vector3 ApplyWaveX(Vector3 vertex)
        {
            vertex.y += WavePower * Mathf.Sin(vertex.x + _weight);
            return vertex;
        }
        
        private Vector3 ApplyWaveY(Vector3 vertex)
        {
            vertex.x += WavePower * Mathf.Sin(vertex.y + _weight);
            return vertex;
        }
        
        private Vector3 ApplyWaveZ(Vector3 vertex)
        {
            vertex.y += WavePower * Mathf.Sin(vertex.z + _weight);
            return vertex;
        }

        /// <summary>
        /// 波动方向
        /// </summary>
        public enum WaveDirection
        {
            /// <summary>
            /// X轴方向
            /// </summary>
            X = 0,
            /// <summary>
            /// Y轴方向
            /// </summary>
            Y = 1,
            /// <summary>
            /// Z轴方向
            /// </summary>
            Z = 2
        }
    }
}