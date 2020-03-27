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
        [SerializeField] protected MeshType _meshType = MeshType.MeshRenderer;
        [SerializeField] protected MeshFilter _meshFilter;
        [SerializeField] protected MeshRenderer _meshRenderer;
        [SerializeField] protected SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] protected bool _isPlayOnStart;
        protected Mesh _mesh;
        protected Material[] _materials;

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; private set; } = false;
        
        private bool _isPlaying = false;
        private Vector3[] _originalVertices;

        protected virtual void Awake()
        {
            IsValid = CheckValidity();

            if (IsValid)
            {
                _mesh = GetMesh();
                _materials = GetMaterials();

                _originalVertices = new Vector3[_mesh.vertices.Length];
                _mesh.vertices.CopyTo(_originalVertices, 0);
            }
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
            if (_isPlaying)
            {
                _mesh.vertices = UpdateEffect(_mesh.vertices);
            }
        }

        protected abstract Vector3[] UpdateEffect(Vector3[] vertices);

        /// <summary>
        /// 播放特效
        /// </summary>
        public virtual void Play()
        {
            if (IsValid)
            {
                _isPlaying = true;
            }
            else
            {
                Debug.LogWarning("当前特效是无效的，无法播放！");
            }
        }

        /// <summary>
        /// 暂停特效
        /// </summary>
        public virtual void Pause()
        {
            _isPlaying = false;
        }

        /// <summary>
        /// 停止特效
        /// </summary>
        /// <param name="isRestoreMesh">是否还原网格为初始状态</param>
        public virtual void Stop(bool isRestoreMesh = false)
        {
            _isPlaying = false;

            if (IsValid && isRestoreMesh)
            {
                Vector3[] vertices = _mesh.vertices;
                _originalVertices.CopyTo(vertices, 0);
                _mesh.vertices = vertices;
            }
        }
        
        private bool CheckValidity()
        {
            if (_meshType == MeshType.MeshRenderer)
            {
                if (_meshFilter == null || _meshRenderer == null || _meshFilter.mesh == null)
                {
                    return false;
                }
            }
            else if (_meshType == MeshType.SkinnedMeshRenderer)
            {
                if (_skinnedMeshRenderer == null || _skinnedMeshRenderer.sharedMesh == null)
                {
                    return false;
                }
            }
            return true;
        }

        private Mesh GetMesh()
        {
            if (_meshType == MeshType.MeshRenderer)
            {
                return _meshFilter.mesh;
            }
            else if (_meshType == MeshType.SkinnedMeshRenderer)
            {
                return _skinnedMeshRenderer.sharedMesh;
            }
            return null;
        }

        private Material[] GetMaterials()
        {
            if (_meshType == MeshType.MeshRenderer)
            {
                return _meshRenderer.materials;
            }
            else if (_meshType == MeshType.SkinnedMeshRenderer)
            {
                return _skinnedMeshRenderer.materials;
            }
            return null;
        }

        /// <summary>
        /// 网格类型
        /// </summary>
        public enum MeshType
        {
            MeshRenderer = 0,
            SkinnedMeshRenderer = 1
        }
    }
}