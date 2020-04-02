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
        public Vector3 Position;
        /// <summary>
        /// 指向网格中的所有三角面（被哪些三角面所持有）
        /// </summary>
        public HashSet<Triangle> Triangles = new HashSet<Triangle>(); 
        /// <summary>
        /// 指向网格中的所有顶点索引
        /// </summary>
        public HashSet<int> Vertices = new HashSet<int>();
    }
}