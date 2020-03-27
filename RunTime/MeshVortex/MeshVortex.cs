using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格涡流特效
    /// </summary>
    [DisallowMultipleComponent]
    public class MeshVortex : MeshEffectsBase
    {
        public float Gravitation = 1;
        public float CentrifugalForce = 1;
        public Vector3 Center;
        public float Radius = 0.5f;

        private GameObject _centerArea;
        private Mesh _centerAreaMesh;
        private Material _centerAreaMaterial;
        private Vector3 _realCenter;

        protected override void Awake()
        {
            base.Awake();

            if (IsValid)
            {
                _centerArea = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                _centerArea.name = "涡流中心";
                _centerArea.transform.position = Center;
                _centerArea.transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
                _centerArea.SetActive(false);
                _centerAreaMesh = _centerArea.GetComponent<MeshFilter>().mesh;
                _centerAreaMaterial = _materials.Length > 0 ? _materials[0] : null;
            }
        }

        protected override Vector3[] UpdateEffect(Vector3[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                float dis = Vector3.Distance(vertices[i], _realCenter);
                if (dis > Radius)
                    vertices[i] = ToBeNearCenter(vertices[i], dis);
            }
            return vertices;
        }
        
        private Vector3 ToBeNearCenter(Vector3 vertex, float factor)
        {
            vertex += (_realCenter - vertex).normalized * (Gravitation / factor);
            return vertex;
        }

        public override void Play()
        {
            base.Play();

            _realCenter = transform.worldToLocalMatrix.MultiplyPoint3x4(Center);
        }
    }
}