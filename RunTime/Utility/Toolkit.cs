using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 工具箱
    /// </summary>
    public static class Toolkit
    {
        #region 网格工具
        private static WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();

        /// <summary>
        /// 下一帧执行操作
        /// </summary>
        /// <param name="behaviour">行为对象实例</param>
        /// <param name="action">操作</param>
        public static void NextFrameExecute(this MonoBehaviour behaviour, Action action)
        {
            behaviour.StartCoroutine(NextFrameExecute(action));
        }
        private static IEnumerator NextFrameExecute(Action action)
        {
            yield return EndOfFrame;

            action();
        }

        /// <summary>
        /// 纠正网格中心
        /// </summary>
        /// <param name="gameObject">目标物体</param>
        /// <param name="mesh">目标网格</param>
        public static void CorrectMeshCenter(this GameObject gameObject, Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;
            Vector3 center = mesh.bounds.center;
            Vector3 offset = gameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(gameObject.transform.position) - center;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += offset;
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            gameObject.transform.position = gameObject.transform.localToWorldMatrix.MultiplyPoint3x4(center);
        }
        #endregion

        #region 反射工具
        /// <summary>
        /// 当前的运行时程序集
        /// </summary>
        private static readonly HashSet<string> RunTimeAssemblies = new HashSet<string>() {
            "Assembly-CSharp", "MeshEditor.Effects.RunTime", "UnityEngine", "UnityEngine.CoreModule", "UnityEngine.UI", "UnityEngine.PhysicsModule" };
        /// <summary>
        /// 从当前程序域的运行时程序集中获取所有类型
        /// </summary>
        /// <returns>所有类型集合</returns>
        public static List<Type> GetTypesInRunTimeAssemblies()
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblys.Length; i++)
            {
                if (RunTimeAssemblies.Contains(assemblys[i].GetName().Name))
                {
                    types.AddRange(assemblys[i].GetTypes());
                }
            }
            return types;
        }
        /// <summary>
        /// 从当前程序域的运行时程序集中获取指定类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>类型</returns>
        public static Type GetTypeInRunTimeAssemblies(string typeName)
        {
            Type type = null;
            foreach (string assembly in RunTimeAssemblies)
            {
                type = Type.GetType(typeName + "," + assembly);
                if (type != null)
                {
                    return type;
                }
            }
            Debug.LogError("获取类型 " + typeName + " 失败！当前运行时程序集中不存在此类型！");
            return null;
        }

        /// <summary>
        /// 从当前程序域的所有程序集中获取所有类型
        /// </summary>
        /// <returns>所有类型集合</returns>
        public static List<Type> GetTypesInAllAssemblies()
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblys.Length; i++)
            {
                types.AddRange(assemblys[i].GetTypes());
            }
            return types;
        }
        #endregion
    }
}