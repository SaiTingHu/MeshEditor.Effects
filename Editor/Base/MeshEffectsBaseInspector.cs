using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MeshEditor.Effects
{
    internal abstract class MeshEffectsBaseInspector<M> : Editor where M : MeshEffectsBase
    {
        protected M Target;

        private Dictionary<string, SerializedProperty> _serializedPropertys = new Dictionary<string, SerializedProperty>();
        private bool _isValid = false;
        private Color _orange = new Color(1, 0.4f, 0, 1);

        protected virtual string HeaderName
        {
            get
            {
                return "MESH EFFECTS";
            }
        }

        protected virtual void OnEnable()
        {
            Target = target as M;

            _isValid = Target.GetComponent<MeshRenderer>() || Target.GetComponent<SkinnedMeshRenderer>();
        }

        public sealed override void OnInspectorGUI()
        {
            if (!_isValid)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("This object doesn't have MeshRenderer or SkinnedMeshRenderer! so mesh effects will not be supported!", MessageType.Error);
                GUILayout.EndHorizontal();
            }
            
            GUILayout.BeginHorizontal("AC BoldHeader");
            GUILayout.FlexibleSpace();
            GUI.color = _orange;
            GUILayout.Label(HeaderName, EditorStyles.boldLabel);
            GUI.color = Color.white;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            PropertyField("_isPlayOnStart", "Play On Start");

            GUILayout.BeginVertical("Box");
            OnMeshEffectsGUI();
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnMeshEffectsGUI()
        {

        }

        /// <summary>
        /// 标记目标已改变
        /// </summary>
        protected void HasChanged()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.SetDirty(target);
                Component component = target as Component;
                if (component != null && component.gameObject.scene != null)
                {
                    EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
                }
            }
        }

        /// <summary>
        /// 根据名字获取序列化属性
        /// </summary>
        /// <param name="propertyName">序列化属性名字</param>
        /// <returns>序列化属性</returns>
        protected SerializedProperty GetProperty(string propertyName)
        {
            SerializedProperty serializedProperty;
            if (_serializedPropertys.ContainsKey(propertyName))
            {
                serializedProperty = _serializedPropertys[propertyName];
            }
            else
            {
                serializedProperty = serializedObject.FindProperty(propertyName);
                if (serializedProperty != null)
                {
                    _serializedPropertys.Add(propertyName, serializedProperty);
                }
            }
            return serializedProperty;
        }
        /// <summary>
        /// 制作一个PropertyField
        /// </summary>
        protected void PropertyField(string propertyName, string name, params GUILayoutOption[] options)
        {
            SerializedProperty serializedProperty = GetProperty(propertyName);

            if (serializedProperty != null)
            {
                EditorGUILayout.PropertyField(serializedProperty, new GUIContent(name), true, options);
            }
            else
            {
                EditorGUILayout.HelpBox("Property [" + propertyName + "] not found!", MessageType.Error);
            }
        }
        /// <summary>
        /// 制作一个PropertyField
        /// </summary>
        protected void PropertyField(string propertyName, params GUILayoutOption[] options)
        {
            SerializedProperty serializedProperty = GetProperty(propertyName);

            if (serializedProperty != null)
            {
                EditorGUILayout.PropertyField(serializedProperty, true, options);
            }
            else
            {
                EditorGUILayout.HelpBox("Property [" + propertyName + "] not found!", MessageType.Error);
            }
        }
        /// <summary>
        /// 制作一个PropertyField
        /// </summary>
        protected void PropertyField(string propertyName, string name, bool includeChildren, params GUILayoutOption[] options)
        {
            SerializedProperty serializedProperty = GetProperty(propertyName);

            if (serializedProperty != null)
            {
                EditorGUILayout.PropertyField(serializedProperty, new GUIContent(name), includeChildren, options);
            }
            else
            {
                EditorGUILayout.HelpBox("Property [" + propertyName + "] not found!", MessageType.Error);
            }
        }
        /// <summary>
        /// 制作一个PropertyField
        /// </summary>
        protected void PropertyField(string propertyName, bool includeChildren, params GUILayoutOption[] options)
        {
            SerializedProperty serializedProperty = GetProperty(propertyName);

            if (serializedProperty != null)
            {
                EditorGUILayout.PropertyField(serializedProperty, includeChildren, options);
            }
            else
            {
                EditorGUILayout.HelpBox("Property [" + propertyName + "] not found!", MessageType.Error);
            }
        }
    }
}