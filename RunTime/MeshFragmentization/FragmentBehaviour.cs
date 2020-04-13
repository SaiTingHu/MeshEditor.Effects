using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格面片行为对象
    /// </summary>
    [DisallowMultipleComponent]
    public class FragmentBehaviour : MonoBehaviour
    {
        protected MeshFragmentization _owner { get; private set; }
        protected Fragment _fragmentData { get; private set; }
        protected Mesh _mesh { get; private set; }
        protected Material[] _materials { get; private set; }
        protected MeshFilter _meshFilter { get; private set; }
        protected MeshRenderer _meshRenderer { get; private set; }

        protected Vector3 _moveDirection;
        protected float _moveSpeed;
        protected Vector3 _rotateDirection;
        protected float _rotateSpeed;
        protected float _healthPoint;

        private Vector3 _moveValue;
        private Quaternion _rotateValue;

        protected virtual void Update()
        {
            if (!_owner.IsPaused)
            {
                if (_healthPoint > 0)
                {
                    _healthPoint -= Time.deltaTime;

                    transform.position += _moveValue * Time.deltaTime;
                    transform.rotation *= _rotateValue;
                }
                else
                {
                    _owner.RecycleFragment(this);
                }
            }
        }
        
        public virtual void Activate(MeshFragmentization owner, Triangle triangle, Material[] materials, float healthPoint, float speed)
        {
            _owner = owner;

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

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            _moveDirection = triangle.Normal;
            _moveSpeed = speed;
            _rotateDirection.Set(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            _rotateSpeed = Random.Range(1f, 10f);
            _healthPoint = healthPoint;

            _moveValue = _moveDirection * _moveSpeed;
            _rotateValue = Quaternion.AngleAxis(_rotateSpeed, _rotateDirection);

            gameObject.CorrectMeshCenter(_mesh);
        }
    }
}