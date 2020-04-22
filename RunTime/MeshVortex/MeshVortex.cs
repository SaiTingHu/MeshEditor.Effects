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
        /// 涡流中心的向心力大小
        /// </summary>
        public float CentripetalForce = 1;
        /// <summary>
        /// 涡流中心的向心力方向
        /// </summary>
        public Vector3 CentripetalDirection = new Vector3(0, 90, 0);
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

            _centerArea = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _centerArea.name = "Vortex Center";
            _centerArea.transform.position = Center;
            _centerArea.transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
            _centerArea.GetComponent<MeshRenderer>().material = _materials.Length > 0 ? _materials[0] : null;
            _centerArea.transform.SetParent(transform);
            _centerArea.SetActive(false);
            _centerAreaForce = Quaternion.AngleAxis(CentripetalForce, CentripetalDirection);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Destroy(_centerArea);
            _centerArea = null;
        }
        
        protected override void OnBeginEffect(MeshData meshData)
        {
            _realCenter = transform.worldToLocalMatrix.MultiplyPoint3x4(Center);
            _centerArea.transform.position = Center;
            _centerArea.SetActive(true);
        }

        protected override void OnUpdateEffect(MeshData meshData)
        {
            _centerArea.transform.rotation *= _centerAreaForce;

            for (int i = 0; i < meshData.Vertices.Count; i++)
            {
                Vertex vertex = meshData.Vertices[i];
                float distance = Vector3.Distance(vertex.Position, _realCenter);
                if (distance > Radius)
                {
                    vertex.Position = ApplyGravitation(vertex.Position, distance + Time.deltaTime);
                    vertex.Position = ApplyCentripetal(vertex.Position, distance + Time.deltaTime);
                }
            }
        }

        protected override void OnEndEffect(MeshData meshData)
        {
            _centerArea.SetActive(false);
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