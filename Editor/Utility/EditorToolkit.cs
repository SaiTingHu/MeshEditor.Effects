using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MeshEditor.Effects
{
    /// <summary>
    /// 编辑器工具箱
    /// </summary>
    public static class EditorToolkit
    {
        #region 层级视图新建菜单
        /// <summary>
        /// 新建Blow特效
        /// </summary>
        [@MenuItem("GameObject/MeshEditor/Effects/Blow", false, 0)]
        private static void CreateBlow()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.name = "New Blow";
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
            }

            obj.AddComponent<MeshBlow>();
            Selection.activeGameObject = obj;
            EditorSceneManager.MarkSceneDirty(obj.scene);
        }

        /// <summary>
        /// 新建Fragmentization特效
        /// </summary>
        [@MenuItem("GameObject/MeshEditor/Effects/Fragmentization", false, 1)]
        private static void CreateFragmentization()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.name = "New Fragmentization";
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
            }

            obj.AddComponent<MeshFragmentization>();
            Selection.activeGameObject = obj;
            EditorSceneManager.MarkSceneDirty(obj.scene);
        }
        
        /// <summary>
        /// 新建Vortex特效
        /// </summary>
        [@MenuItem("GameObject/MeshEditor/Effects/Vortex", false, 2)]
        private static void CreateVortex()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.name = "New Vortex";
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
            }

            obj.AddComponent<MeshVortex>();
            Selection.activeGameObject = obj;
            EditorSceneManager.MarkSceneDirty(obj.scene);
        }

        /// <summary>
        /// 新建Wave特效
        /// </summary>
        [@MenuItem("GameObject/MeshEditor/Effects/Wave", false, 3)]
        private static void CreateWave()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                obj.name = "New Wave";
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
            }

            obj.AddComponent<MeshWave>();
            Selection.activeGameObject = obj;
            EditorSceneManager.MarkSceneDirty(obj.scene);
        }
        #endregion
    }
}