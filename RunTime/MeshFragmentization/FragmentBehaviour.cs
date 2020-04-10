using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格面片行为对象
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class FragmentBehaviour : MonoBehaviour
    {
        private Fragment _fragmentData;
        private Mesh _mesh;
        private Material[] _materials;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        
        private void Update()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialization(Triangle triangle, Material[] materials)
        {
            if (_fragmentData == null) _fragmentData = new Fragment();
            _fragmentData.Clear();
            _fragmentData.AppendTriangle(triangle);

            if (_mesh == null) _mesh = new Mesh();
            _fragmentData.ApplyData(_mesh);
            _materials = materials;
            
            if (_meshFilter == null)
            {
                _meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            _meshFilter.mesh = _mesh;

            if (_meshRenderer == null)
            {
                _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
            _meshRenderer.materials = _materials;
        }
    }
}