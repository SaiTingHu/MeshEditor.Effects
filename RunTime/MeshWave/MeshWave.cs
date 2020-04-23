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
        /// 扁平模式
        /// </summary>
        public bool IsFlat = false;
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

        protected override void OnBeginEffect(MeshData meshData)
        {
            switch (Direction)
            {
                case WaveDirection.X:
                    if (IsFlat) _applyWave = ApplyWaveFlatX;
                    else _applyWave = ApplyWaveX;
                    break;
                case WaveDirection.Y:
                    if (IsFlat) _applyWave = ApplyWaveFlatY;
                    else _applyWave = ApplyWaveY;
                    break;
                case WaveDirection.Z:
                    if (IsFlat) _applyWave = ApplyWaveFlatZ;
                    else _applyWave = ApplyWaveZ;
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

        protected override void OnUpdateEffect(MeshData meshData)
        {
            UpdateWeight();

            for (int i = 0; i < meshData.Vertices.Count; i++)
            {
                Vertex vertex = meshData.Vertices[i];
                vertex.Position = _applyWave(_positions[i]);
            }
        }

        protected override void OnEndEffect(MeshData meshData)
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

        private Vector3 ApplyWaveFlatX(Vector3 vertex)
        {
            vertex.y = WavePower * Mathf.Sin(vertex.x + _weight);
            return vertex;
        }

        private Vector3 ApplyWaveFlatY(Vector3 vertex)
        {
            vertex.x = WavePower * Mathf.Sin(vertex.y + _weight);
            return vertex;
        }

        private Vector3 ApplyWaveFlatZ(Vector3 vertex)
        {
            vertex.y = WavePower * Mathf.Sin(vertex.z + _weight);
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