using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格碎化特效
    /// </summary>
    [DisallowMultipleComponent]
    public class MeshFragmentization : MeshEffectsBase
    {
        /// <summary>
        /// 碎化点
        /// </summary>
        public Vector3 FragPoint;
        /// <summary>
        /// 碎化间隔时间
        /// </summary>
        public float IntervalTime = 0.2f;
        /// <summary>
        /// 碎片行为类型
        /// </summary>
        public string FragmentType = "MeshEditor.Effects.FragmentBehaviour";

        private List<Triangle> _trianglesOrder = new List<Triangle>();
        private HashSet<Triangle> _triangleOpen = new HashSet<Triangle>();
        private HashSet<Triangle> _triangleReady = new HashSet<Triangle>();
        private HashSet<Triangle> _triangleClose = new HashSet<Triangle>();
        private Queue<FragmentBehaviour> _fragmentPool = new Queue<FragmentBehaviour>();
        private float _timer = 0;
        private Type _fragmentType;

        protected override void Awake()
        {
            base.Awake();

            _fragmentType = Toolkit.GetTypeInRunTimeAssemblies(FragmentType);
        }

        protected override void BeginEffect(MeshData meshData)
        {
            if (IntervalTime < 0)
            {
                IntervalTime = 0;
            }

            GetTrianglesOrder(GetFirstTriangle(meshData));
        }

        protected override void UpdateEffect(MeshData meshData)
        {
            if (_timer < IntervalTime)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _timer = 0;
                Fragmentization(meshData);
            }
        }

        //获取第一个三角面，碎化起点
        private Triangle GetFirstTriangle(MeshData meshData)
        {
            Triangle triangle = meshData.Triangles[0];
            float distance = Vector3.Distance(FragPoint, meshData.Triangles[0].Center);
            for (int i = 1; i < meshData.Triangles.Count; i++)
            {
                float dis = Vector3.Distance(FragPoint, meshData.Triangles[i].Center);
                if (dis < distance)
                {
                    triangle = meshData.Triangles[i];
                    distance = dis;
                }
            }
            return triangle;
        }

        //获取三角面碎化顺序算法
        private void GetTrianglesOrder(Triangle first)
        {
            _trianglesOrder.Clear();
            _triangleOpen.Clear();
            _triangleReady.Clear();
            _triangleClose.Clear();

            //将第一个三角面加入准备列表
            _triangleReady.Add(first);

            //如果还有准备中的三角面
            while (_triangleReady.Count > 0)
            {
                //将准备中的三角面纳入开启列表
                _triangleOpen.UnionWith(_triangleReady);
                _triangleReady.Clear();

                //遍历开启列表，逐一关闭
                foreach (var item in _triangleOpen)
                {
                    CloseTriangle(item);
                }

                //遍历开启列表，逐一将其邻居纳入准备列表
                foreach (var item in _triangleOpen)
                {
                    ReadyNeighborTriangle(item);
                }

                _triangleOpen.Clear();
            }

            _triangleOpen.Clear();
            _triangleReady.Clear();
            _triangleClose.Clear();
        }

        //关闭三角面，三角面加入关闭列表
        private void CloseTriangle(Triangle triangle)
        {
            if (!_triangleClose.Contains(triangle))
            {
                _triangleClose.Add(triangle);
                _trianglesOrder.Add(triangle);
                triangle.BrokenLinkVertex();
            }
        }

        //准备相邻三角面，三角面加入准备列表
        private void ReadyNeighborTriangle(Triangle triangle)
        {
            foreach (var item in triangle.Vertex1.Triangles)
            {
                _triangleReady.Add(item);
            }
            foreach (var item in triangle.Vertex2.Triangles)
            {
                _triangleReady.Add(item);
            }
            foreach (var item in triangle.Vertex3.Triangles)
            {
                _triangleReady.Add(item);
            }
        }

        //碎化
        private void Fragmentization(MeshData meshData)
        {
            if (_trianglesOrder.Count > 0)
            {
                Triangle triangle = _trianglesOrder[0];
                _trianglesOrder.RemoveAt(0);
                meshData.RemoveTriangle(triangle);
                GenerateFragment(triangle);
            }
            else
            {
                Stop(false);
            }
        }

        //生成碎片行为对象
        private void GenerateFragment(Triangle triangle)
        {
            FragmentBehaviour fragmentBehaviour;
            if (_fragmentPool.Count > 0)
            {
                fragmentBehaviour = _fragmentPool.Dequeue();
            }
            else
            {
                GameObject obj = new GameObject("Fragment");
                obj.transform.SetParent(transform);
                fragmentBehaviour = obj.AddComponent(_fragmentType) as FragmentBehaviour;
            }

            fragmentBehaviour.gameObject.SetActive(true);
            fragmentBehaviour.Activate(this, triangle, _materials);
        }

        /// <summary>
        /// 回收碎片行为对象
        /// </summary>
        /// <param name="fragment">碎片</param>
        public void RecycleFragment(FragmentBehaviour fragment)
        {
            _fragmentPool.Enqueue(fragment);
            fragment.gameObject.SetActive(false);
        }
    }
}