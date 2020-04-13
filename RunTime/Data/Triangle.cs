using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 三角面
    /// </summary>
    public sealed class Triangle : MeshElement
    {
        /// <summary>
        /// 顶点1
        /// </summary>
        public Vertex Vertex1 { get; private set; }
        /// <summary>
        /// 顶点2
        /// </summary>
        public Vertex Vertex2 { get; private set; }
        /// <summary>
        /// 顶点3
        /// </summary>
        public Vertex Vertex3 { get; private set; }
        /// <summary>
        /// 顶点1的索引
        /// </summary>
        public int Vertex1Index { get; private set; }
        /// <summary>
        /// 顶点2的索引
        /// </summary>
        public int Vertex2Index { get; private set; }
        /// <summary>
        /// 顶点3的索引
        /// </summary>
        public int Vertex3Index { get; private set; }

        /// <summary>
        /// 中心点
        /// </summary>
        public Vector3 Center
        {
            get
            {
                return ((Vertex1.Position + Vertex2.Position) / 2 + Vertex3.Position) / 2;
            }
        }

        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 Normal
        {
            get
            {
                return (Vertex1.Normal + Vertex2.Normal + Vertex3.Normal).normalized;
            }
        }

        public Triangle(Vertex vertex1, Vertex vertex2, Vertex vertex3, int index1, int index2, int index3)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
            Vertex1Index = index1;
            Vertex2Index = index2;
            Vertex3Index = index3;

            LinkVertex();
        }

        /// <summary>
        /// 链接所有顶点
        /// </summary>
        public void LinkVertex()
        {
            Vertex1.Triangles.Add(this);
            Vertex2.Triangles.Add(this);
            Vertex3.Triangles.Add(this);
        }

        /// <summary>
        /// 断开所有顶点链接
        /// </summary>
        public void BrokenLinkVertex()
        {
            Vertex1.Triangles.Remove(this);
            Vertex2.Triangles.Remove(this);
            Vertex3.Triangles.Remove(this);
        }
    }
}