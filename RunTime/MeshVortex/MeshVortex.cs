using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格涡流特效
    /// </summary>
    [DisallowMultipleComponent]
    public class MeshVortex : MeshEffectsBase
    {
        /// <summary>
        /// 涡流中心的引力大小
        /// </summary>
        public float Gravitation = 1;
        /// <summary>
        /// 涡流中心的向心力方向
        /// </summary>
        public Vector3 CentripetalDirection = new Vector3(0, 90, 0);
        /// <summary>
        /// 涡流中心的向心力大小
        /// </summary>
        public float CentripetalForce = 1;
        /// <summary>
        /// 涡流中心
        /// </summary>
        public Vector3 Center;
        /// <summary>
        /// 涡流中心凝聚半径
        /// </summary>
        public float Radius = 0.5f;
        
        private Vector3 _realCenter;
        private GameObject _centerArea;
        private Quaternion _centerAreaForce;

        protected override void Awake()
        {
            base.Awake();

            if (IsValid)
            {
                _centerArea = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                _centerArea.name = "Vortex Center";
                _centerArea.transform.position = Center;
                _centerArea.transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
                _centerArea.GetComponent<MeshRenderer>().material = _materials.Length > 0 ? _materials[0] : null;
                _centerArea.SetActive(false);
                _centerAreaForce = Quaternion.AngleAxis(CentripetalForce, CentripetalDirection);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (IsValid)
            {
                Destroy(_centerArea);
                _centerArea = null;
            }
        }

        public override void Play()
        {
            base.Play();

            _realCenter = transform.worldToLocalMatrix.MultiplyPoint3x4(Center);
            _centerArea.SetActive(true);
        }

        public override void Stop(bool isRestoreMesh = false)
        {
            base.Stop(isRestoreMesh);

            _centerArea.SetActive(false);
        }

        protected override Vector3[] UpdateEffect(Vector3[] vertices)
        {
            _centerArea.transform.rotation *= _centerAreaForce;

            for (int i = 0; i < vertices.Length; i++)
            {
                float distance = Vector3.Distance(vertices[i], _realCenter);
                if (distance > Radius)
                {
                    vertices[i] = ApplyGravitation(vertices[i], distance + Time.deltaTime);
                    vertices[i] = ApplyCentripetal(vertices[i], distance + Time.deltaTime);
                }
            }
            return vertices;
        }

        //应用引力
        private Vector3 ApplyGravitation(Vector3 vertex, float factor)
        {
            float gravitation = Gravitation / factor * Time.deltaTime;
            return vertex + (_realCenter - vertex).normalized * (gravitation > factor ? factor : gravitation);
        }
        //应用向心力
        private Vector3 ApplyCentripetal(Vector3 vertex, float factor)
        {
            return Quaternion.AngleAxis(CentripetalForce / factor, CentripetalDirection) * (vertex - _realCenter) + _realCenter;
        }
    }
}