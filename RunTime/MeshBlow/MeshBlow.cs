using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格疾风特效
    /// </summary>
    [DisallowMultipleComponent]
    public class MeshBlow : MeshEffectsBase
    {
        /// <summary>
        /// 风力
        /// </summary>
        public float WindPower = 1;
        /// <summary>
        /// 风力变化区间最小比例
        /// </summary>
        public float WindPowerMinRate = 0.0f;
        /// <summary>
        /// 风力变化区间最大比例
        /// </summary>
        public float WindPowerMaxRate = 1.0f;

        [SerializeField] private Vector3 _blowsDirection = new Vector3(0, 0, -1);
        private bool _isGenerate = false;
        private float[] _dots;
        private Vector3[] _positions;
        
        /// <summary>
        /// 风向
        /// </summary>
        public Vector3 BlowsDirection
        {
            get
            {
                return _blowsDirection;
            }
            set
            {
                _blowsDirection = value.normalized;
            }
        }
        
        protected override void OnBeginEffect(MeshData meshData)
        {
            if (!_isGenerate)
            {
                _isGenerate = true;
                _dots = new float[meshData.Vertices.Count];
                _positions = new Vector3[meshData.Vertices.Count];
                for (int i = 0; i < meshData.Vertices.Count; i++)
                {
                    _dots[i] = Vector3.Dot(meshData.Vertices[i].Normal, _blowsDirection);
                    _positions[i] = meshData.Vertices[i].Position;
                }
            }
        }

        protected override void OnUpdateEffect(MeshData meshData)
        {
            for (int i = 0; i < meshData.Vertices.Count; i++)
            {
                if (_dots[i] <= 0)
                {
                    Vertex vertex = meshData.Vertices[i];
                    vertex.Position = ApplyWindPower(_positions[i], _dots[i]);
                }
            }
        }

        protected override void OnEndEffect(MeshData meshData)
        {
            
        }

        //应用风力
        private Vector3 ApplyWindPower(Vector3 vertex, float factor)
        {
            return vertex + _blowsDirection * WindPower * factor * Random.Range(WindPowerMinRate, WindPowerMaxRate);
        }
    }
}