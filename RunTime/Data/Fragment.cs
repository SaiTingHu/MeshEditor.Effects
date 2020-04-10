using System.Collections.Generic;
using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格面片
    /// </summary>
    public sealed class Fragment : MeshElement
    {
        private HashSet<Triangle> _triangles;
        private HashSet<Vertex> _vertices;
        private Dictionary<Vertex, int> _signer;

        public Fragment()
        {
            _triangles = new HashSet<Triangle>();
            _vertices = new HashSet<Vertex>();
            _signer = new Dictionary<Vertex, int>();
        }

        /// <summary>
        /// 向面片中附加一个三角面
        /// </summary>
        /// <param name="triangle">三角面</param>
        /// <returns>是否附加成功，附加失败时，可能面片中已经存在该三角面</returns>
        public bool AppendTriangle(Triangle triangle)
        {
            bool succeed = _triangles.Add(triangle);
            if (succeed)
            {
                _vertices.Add(triangle.Vertex1);
                _vertices.Add(triangle.Vertex2);
                _vertices.Add(triangle.Vertex3);
            }
            return succeed;
        }

        /// <summary>
        /// 清空面片数据
        /// </summary>
        public void Clear()
        {
            _triangles.Clear();
            _vertices.Clear();
            _signer.Clear();
        }

        /// <summary>
        /// 应用面片数据到 Mesh 对象
        /// </summary>
        /// <param name="mesh">目标对象</param>
        public void ApplyData(Mesh mesh)
        {
            _signer.Clear();
            
            //应用顶点
            Vector3[] vertices = mesh.vertices;
            if (vertices == null || vertices.Length != _vertices.Count)
            {
                vertices = new Vector3[_vertices.Count];
            }
            Vector3[] normals = mesh.normals;
            if (normals == null || normals.Length != _vertices.Count)
            {
                normals = new Vector3[_vertices.Count];
            }
            Vector2[] uv = mesh.uv;
            if (uv == null || uv.Length != _vertices.Count)
            {
                uv = new Vector2[_vertices.Count];
            }
            int index = 0;
            foreach (var item in _vertices)
            {
                vertices[index] = item.Position;
                normals[index] = item.Normal;
                uv[index] = item.UV;
                _signer.Add(item, index);
                index += 1;
            }
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;

            //应用三角面
            int trianglesNumber = _triangles.Count * 3;
            int[] triangles = mesh.triangles;
            if (triangles == null || triangles.Length != trianglesNumber)
            {
                triangles = new int[trianglesNumber];
            }
            index = 0;
            foreach (var item in _triangles)
            {
                triangles[index] = _signer[item.Vertex1];
                triangles[index + 1] = _signer[item.Vertex2];
                triangles[index + 2] = _signer[item.Vertex3];
                index += 3;
            }
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            _signer.Clear();
        }
    }
}