using System.Collections.Generic;
using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 顶点
    /// </summary>
    public sealed class Vertex : MeshElement
    {
        /// <summary>
        /// 顶点位置
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// 顶点法线
        /// </summary>
        public Vector3 Normal { get; set; }
        /// <summary>
        /// 顶点UV
        /// </summary>
        public Vector2 UV { get; set; }
        /// <summary>
        /// 指向网格中的所有三角面（被哪些三角面所持有）
        /// </summary>
        public HashSet<Triangle> Triangles { get; private set; }
        /// <summary>
        /// 指向网格中的所有顶点索引（在网格顶点数组中的索引）
        /// </summary>
        public HashSet<int> Indexs { get; private set; }

        public Vertex(Vector3 position, Vector3 normal, Vector2 uv, HashSet<int> indexs)
        {
            Position = position;
            Normal = normal;
            UV = uv;
            Triangles = new HashSet<Triangle>();
            Indexs = indexs;
        }
    }
}