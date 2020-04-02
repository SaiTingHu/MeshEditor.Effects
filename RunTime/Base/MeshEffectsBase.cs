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
        protected Mesh _mesh { get; private set; }
        protected Material[] _materials { get; private set; }
        
        public bool IsPlaying { get; private set; } = false;

        private MeshData _data;
        
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
                UpdateEffect(_data);
                _mesh.vertices = _data.GetVertices();
            }
        }

        protected virtual void OnDestroy()
        {
            
        }

        protected abstract void UpdateEffect(MeshData meshData);

        /// <summary>
        /// 播放特效
        /// </summary>
        public virtual void Play()
        {
            if (_mesh != null && _data != null)
            {
                if (_skinnedMeshRenderer != null)
                {
                    _skinnedMeshRenderer.enabled = false;
                    _meshRenderer.enabled = true;

                    _skinnedMeshRenderer.BakeMesh(_mesh);
                    _data.ReadData(_mesh.vertices);
                }

                IsPlaying = true;
            }
            else
            {
                Debug.LogWarning("播放网格特效失败：丢失网格数据！");
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

            if (_mesh != null && _data != null && isRestoreMesh)
            {
                _mesh.vertices = _data.GetOriginalVertices();
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
                if (_skinnedMeshRenderer.sharedMesh != null)
                {
                    _mesh = new Mesh();
                    _skinnedMeshRenderer.BakeMesh(_mesh);
                    _meshFilter.mesh = _mesh;
                }
                _materials = _meshRenderer.materials = _skinnedMeshRenderer.materials;
                _meshRenderer.enabled = false;
                
                GenerateMeshData(_mesh);
            }
            else
            {
                _mesh = _meshFilter.mesh;
                _materials = _meshRenderer.materials;
                _meshRenderer.enabled = true;
                
                GenerateMeshData(_mesh);
            }
        }

        private void GenerateMeshData(Mesh mesh)
        {
            if (mesh != null)
            {
                _data = new MeshData(mesh);
            }
        }
    }
}