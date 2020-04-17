using System.Collections.Generic;
using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 网格数据
    /// </summary>
    public sealed class MeshData
    {
        /// <summary>
        /// 所有顶点【请勿进行增、删操作】
        /// </summary>
        public List<Vertex> Vertices { get; private set; }
        /// <summary>
        /// 所有三角面【请勿进行增、删操作】
        /// </summary>
        public List<Triangle> Triangles { get; private set; }
        /// <summary>
        /// 顶点身份标记器，（1, Vertex）表明索引为1的顶点属于Vertex顶点对象【请勿进行增、删操作】
        /// </summary>
        public Dictionary<int, Vertex> Signer { get; private set; }

        private Mesh _mesh;
        private Vector3[] _vertices;
        private Vector3[] _originalVertices;
        private Vector3[] _normals;
        private Vector2[] _uvs;
        private int[] _triangles;
        private int[] _originalTriangles;

        public MeshData(Mesh mesh)
        {
            Vertices = new List<Vertex>();
            Triangles = new List<Triangle>();
            Signer = new Dictionary<int, Vertex>();

            _mesh = mesh;
            _vertices = new Vector3[_mesh.vertexCount];
            _originalVertices = new Vector3[_mesh.vertexCount];
            _normals = new Vector3[_mesh.vertexCount];
            _uvs = new Vector2[_mesh.vertexCount];
            _triangles = new int[_mesh.triangles.Length];
            _originalTriangles = new int[_mesh.triangles.Length];
            _mesh.vertices.CopyTo(_vertices, 0);
            _mesh.vertices.CopyTo(_originalVertices, 0);
            _mesh.normals.CopyTo(_normals, 0);
            _mesh.uv.CopyTo(_uvs, 0);
            _mesh.triangles.CopyTo(_triangles, 0);
            _mesh.triangles.CopyTo(_originalTriangles, 0);

            //生成顶点代理者
            List<VertexAgent> vertexAgents = new List<VertexAgent>();
            for (int i = 0; i < _vertices.Length; i++)
            {
                vertexAgents.Add(new VertexAgent(i, _vertices[i]));
            }

            //生成顶点数据
            while (vertexAgents.Count > 0)
            {
                VertexAgent agent = vertexAgents[0];
                Vector3 position = agent.Position;
                Vector3 normal = _normals[agent.Index];
                Vector2 uv = _uvs[agent.Index];
                HashSet<int> indexs = new HashSet<int>();
                for (int i = 0; i < vertexAgents.Count; i++)
                {
                    if (agent.Position == vertexAgents[i].Position)
                    {
                        indexs.Add(vertexAgents[i].Index);
                        vertexAgents.RemoveAt(i);
                        i -= 1;
                    }
                }
                Vertex vertex = new Vertex(position, normal, uv, indexs);
                Vertices.Add(vertex);

                foreach (int index in indexs)
                {
                    Signer.Add(index, vertex);
                }
            }

            //生成三角面数据
            for (int i = 0; (i + 2) < _triangles.Length; i += 3)
            {
                Vertex vertex1 = Signer[_triangles[i]];
                Vertex vertex2 = Signer[_triangles[i + 1]];
                Vertex vertex3 = Signer[_triangles[i + 2]];
                Triangle triangle = new Triangle(vertex1, vertex2, vertex3, _triangles[i], _triangles[i + 1], _triangles[i + 2]);
                Triangles.Add(triangle);
            }

            vertexAgents = null;
        }

        /// <summary>
        /// 应用数据
        /// </summary>
        public void ApplyData()
        {
            //重新应用顶点
            for (int i = 0; i < Vertices.Count; i++)
            {
                foreach (int index in Vertices[i].Indexs)
                {
                    _vertices[index] = Vertices[i].Position;
                }
            }
            _mesh.vertices = _vertices;

            //重新应用三角面（如果三角面数量改变）
            if ((Triangles.Count * 3) != _triangles.Length)
            {
                _triangles = new int[Triangles.Count * 3];
                for (int i = 0; i < Triangles.Count; i++)
                {
                    _triangles[i * 3] = Triangles[i].Vertex1Index;
                    _triangles[i * 3 + 1] = Triangles[i].Vertex2Index;
                    _triangles[i * 3 + 2] = Triangles[i].Vertex3Index;
                }
                _mesh.triangles = _triangles;
            }
        }

        /// <summary>
        /// 应用数据到初始状态
        /// </summary>
        public void ApplyToOriginal()
        {
            //还原顶点
            _originalVertices.CopyTo(_vertices, 0);
            _mesh.vertices = _vertices;

            //还原三角面
            if (_triangles.Length != _originalTriangles.Length)
            {
                _triangles = new int[_originalTriangles.Length];
            }
            _originalTriangles.CopyTo(_triangles, 0);
            _mesh.triangles = _triangles;
        }

        /// <summary>
        /// 重新读取数据
        /// </summary>
        public void ReadData()
        {
            //重新读取顶点
            _mesh.vertices.CopyTo(_vertices, 0);
            for (int i = 0; i < _vertices.Length; i++)
            {
                Signer[i].Position = _vertices[i];
            }

            //重新读取三角面（如果三角面数量改变）
            if ((Triangles.Count * 3) != _originalTriangles.Length)
            {
                //重新生成三角面数据
                ClearTriangles();
                for (int i = 0; (i + 2) < _originalTriangles.Length; i += 3)
                {
                    Vertex vertex1 = Signer[_originalTriangles[i]];
                    Vertex vertex2 = Signer[_originalTriangles[i + 1]];
                    Vertex vertex3 = Signer[_originalTriangles[i + 2]];
                    Triangle triangle = new Triangle(vertex1, vertex2, vertex3, _originalTriangles[i], _originalTriangles[i + 1], _originalTriangles[i + 2]);
                    Triangles.Add(triangle);
                }
            }
        }

        /// <summary>
        /// 新增三角面
        /// </summary>
        /// <param name="triangle">三角面</param>
        public void AddTriangle(Triangle triangle)
        {
            triangle.LinkVertex();
            Triangles.Add(triangle);
        }

        /// <summary>
        /// 新增三角面
        /// </summary>
        /// <param name="triangles">三角面集合</param>
        public void AddTriangles(List<Triangle> triangles)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                triangles[i].LinkVertex();
                Triangles.Add(triangles[i]);
            }
        }

        /// <summary>
        /// 移除三角面
        /// </summary>
        /// <param name="triangle">三角面</param>
        public void RemoveTriangle(Triangle triangle)
        {
            triangle.BrokenLinkVertex();
            Triangles.Remove(triangle);
        }

        /// <summary>
        /// 移除三角面
        /// </summary>
        /// <param name="index">三角面索引</param>
        public void RemoveAtTriangle(int index)
        {
            Triangles[index].BrokenLinkVertex();
            Triangles.RemoveAt(index);
        }

        /// <summary>
        /// 清空所有三角面
        /// </summary>
        public void ClearTriangles()
        {
            for (int i = 0; i < Triangles.Count; i++)
            {
                Triangles[i].BrokenLinkVertex();
            }
            Triangles.Clear();
        }

        private class VertexAgent
        {
            public int Index;
            public Vector3 Position;

            public VertexAgent(int index, Vector3 position)
            {
                Index = index;
                Position = position;
            }
        }
    }
}