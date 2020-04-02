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
        /// 所有三角面
        /// </summary>
        public List<Triangle> Triangles = new List<Triangle>();
        /// <summary>
        /// 所有顶点
        /// </summary>
        public List<Vertex> Vertices = new List<Vertex>();
        /// <summary>
        /// 顶点身份标记器（<1, Vertex>表明索引为1的顶点属于Vertex顶点对象）
        /// </summary>
        public Dictionary<int, Vertex> Signer = new Dictionary<int, Vertex>();

        private Vector3[] _vertices;
        private Vector3[] _originalVertices;
        private int[] _triangles;
        
        public MeshData(Mesh mesh)
        {
            _vertices = new Vector3[mesh.vertexCount];
            _originalVertices = new Vector3[mesh.vertexCount];
            _triangles = new int[mesh.triangles.Length];
            mesh.vertices.CopyTo(_vertices, 0);
            mesh.vertices.CopyTo(_originalVertices, 0);
            mesh.triangles.CopyTo(_triangles, 0);

            //生成顶点代理者
            List<VertexAgent> vertexAgents = new List<VertexAgent>();
            for (int i = 0; i < _vertices.Length; i++)
            {
                vertexAgents.Add(new VertexAgent(i, _vertices[i]));
            }

            //生成顶点数据
            while (vertexAgents.Count > 0)
            {
                Vertex vertex = new Vertex();
                vertex.Position = vertexAgents[0].Position;
                for (int i = 0; i < vertexAgents.Count; i++)
                {
                    if (vertex.Position == vertexAgents[i].Position)
                    {
                        vertex.Vertices.Add(vertexAgents[i].Index);
                        Signer.Add(vertexAgents[i].Index, vertex);
                        vertexAgents.RemoveAt(i);
                        i -= 1;
                    }
                }
                Vertices.Add(vertex);
            }

            //生成三角面数据
            for (int i = 0; (i + 2) < _triangles.Length; i += 3)
            {
                Triangle triangle = new Triangle();
                Vertex vertex1 = Signer[_triangles[i]];
                Vertex vertex2 = Signer[_triangles[i + 1]];
                Vertex vertex3 = Signer[_triangles[i + 2]];
                if (!vertex1.Triangles.Contains(triangle)) vertex1.Triangles.Add(triangle);
                if (!vertex2.Triangles.Contains(triangle)) vertex2.Triangles.Add(triangle);
                if (!vertex3.Triangles.Contains(triangle)) vertex3.Triangles.Add(triangle);
                triangle.Vertex1 = vertex1;
                triangle.Vertex2 = vertex2;
                triangle.Vertex3 = vertex3;
                Triangles.Add(triangle);
            }

            vertexAgents = null;
        }

        /// <summary>
        /// 获取标准化后的顶点数组
        /// </summary>
        /// <returns>顶点数组</returns>
        public Vector3[] GetVertices()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                foreach (int index in Vertices[i].Vertices)
                {
                    _vertices[index] = Vertices[i].Position;
                }
            }
            return _vertices;
        }

        /// <summary>
        /// 获取初始时的顶点数组
        /// </summary>
        /// <returns>顶点数组</returns>
        public Vector3[] GetOriginalVertices()
        {
            _originalVertices.CopyTo(_vertices, 0);
            return _vertices;
        }

        /// <summary>
        /// 重新读取顶点数据
        /// </summary>
        /// <param name="vertices">顶点数组</param>
        public void ReadData(Vector3[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Signer[i].Position = vertices[i];
            }
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