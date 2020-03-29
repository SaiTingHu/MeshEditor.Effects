using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格特效基类
    /// </summary>
    public abstract class MeshEffectsBase : MonoBehaviour
    {
        [SerializeField] protected bool _isPlayOnStart = true;
        protected SkinnedMeshRenderer _skinnedMeshRenderer { get; private set; }
        protected MeshFilter _meshFilter { get; private set; }
        protected MeshRenderer _meshRenderer { get; private set; }
        protected Vector3[] _originalVertices { get; private set; }
        protected Mesh _mesh { get; private set; }
        protected Material[] _materials { get; private set; }
        
        public bool IsPlaying { get; private set; } = false;
        
        protected virtual void Awake()
        {
            Initialization();
        }

        protected virtual void Start()
        {
            if (_isPlayOnStart)
            {
                Play();
            }
        }

        protected virtual void Update()
        {
            if (IsPlaying)
            {
                _mesh.vertices = UpdateEffect(_mesh.vertices);
            }
        }

        protected virtual void OnDestroy()
        {
            
        }

        protected abstract Vector3[] UpdateEffect(Vector3[] vertices);

        /// <summary>
        /// 播放特效
        /// </summary>
        public virtual void Play()
        {
            if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.enabled = false;
                _meshRenderer.enabled = true;

                _mesh = _meshFilter.mesh;
                if (_mesh == null) _mesh = new Mesh();
                _skinnedMeshRenderer.BakeMesh(_mesh);
                _meshFilter.mesh = _mesh;
            }
            
            if (_mesh != null)
            {
                IsPlaying = true;
            }
            else
            {
                Debug.LogWarning("播放网格涡流特效失败：丢失网格数据！");
            }
        }

        /// <summary>
        /// 暂停特效
        /// </summary>
        public virtual void Pause()
        {
            IsPlaying = false;
        }

        /// <summary>
        /// 停止特效
        /// </summary>
        /// <param name="isRestoreMesh">是否还原网格为初始状态</param>
        public virtual void Stop(bool isRestoreMesh = true)
        {
            if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.enabled = true;
                _meshRenderer.enabled = false;
            }

            IsPlaying = false;

            if (_mesh != null && _originalVertices != null && isRestoreMesh)
            {
                Vector3[] vertices = _mesh.vertices;
                _originalVertices.CopyTo(vertices, 0);
                _mesh.vertices = vertices;
            }
        }

        private void Initialization()
        {
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            _meshFilter = GetComponent<MeshFilter>();
            if (_meshFilter == null) _meshFilter = gameObject.AddComponent<MeshFilter>();

            _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer == null) _meshRenderer = gameObject.AddComponent<MeshRenderer>();

            if (_skinnedMeshRenderer != null)
            {
                _materials = _meshRenderer.materials = _skinnedMeshRenderer.materials;
                _meshRenderer.enabled = false;
            }
            else
            {
                _mesh = _meshFilter.mesh;
                _materials = _meshRenderer.materials;
                _meshRenderer.enabled = true;

                if (_mesh != null)
                {
                    _originalVertices = new Vector3[_mesh.vertices.Length];
                    _mesh.vertices.CopyTo(_originalVertices, 0);
                }
            }
        }
    }
}