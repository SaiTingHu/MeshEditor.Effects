using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格变形碎片行为对象
    /// </summary>
    [DisallowMultipleComponent]
    public class TransformerBehaviour : MonoBehaviour
    {
        protected MeshTransformer _owner { get; private set; }
        protected Fragment _data { get; private set; }
        protected Mesh _mesh { get; private set; }
        protected Material[] _materials { get; private set; }
        protected MeshFilter _meshFilter { get; private set; }
        protected MeshRenderer _meshRenderer { get; private set; }

        protected Vector3 _moveDirection;
        protected float _moveSpeed;
        protected Vector3 _rotateDirection;
        protected float _rotateSpeed;

        private Vector3 _moveValue;
        private Quaternion _rotateValue;

        protected virtual void Update()
        {
            /*if (!_owner.IsPaused)
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
            }*/
        }

        public virtual void Activate(MeshTransformer owner, Triangle triangle, Material[] materials, float moveSpeed, float rotateSpeed)
        {
            _owner = owner;

            if (_data == null) _data = new Fragment();
            _data.Clear();
            _data.AppendTriangle(triangle);

            if (_mesh == null) _mesh = new Mesh();
            _data.ApplyData(_mesh);
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
            _moveSpeed = moveSpeed;
            _rotateDirection.Set(0, 90, 0);
            _rotateSpeed = Random.Range(1f, 10f);

            _moveValue = _moveDirection * _moveSpeed;
            _rotateValue = Quaternion.AngleAxis(_rotateSpeed, _rotateDirection);

            gameObject.CorrectMeshCenter(_mesh);
        }
    }
}