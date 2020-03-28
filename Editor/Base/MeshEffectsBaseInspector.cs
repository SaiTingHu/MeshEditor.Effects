using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MeshEditor.Effects
{
    internal abstract class MeshEffectsBaseInspector<M> : Editor where M : MeshEffectsBase
    {
        protected M Target;

        private Dictionary<string, SerializedProperty> _serializedPropertys = new Dictionary<string, SerializedProperty>();
        private SerializedProperty _meshType;
        private SerializedProperty _meshFilter;
        private SerializedProperty _meshRenderer;
        private SerializedProperty _skinnedMeshRenderer;
        private int _meshTypeCache;

        protected virtual void OnEnable()
        {
            Target = target as M;
            _meshType = GetProperty("_meshType");
            _meshFilter = GetProperty("_meshFilter");
            _meshRenderer = GetProperty("_meshRenderer");
            _skinnedMeshRenderer = GetProperty("_skinnedMeshRenderer");
            _meshTypeCache = -1;
        }

        public sealed override void OnInspectorGUI()
        {
            PropertyField("_isPlayOnStart", "Play On Start");
            PropertyField("_meshType", "Mesh Type");

            GUI.enabled = false;
            if (_meshType.enumValueIndex == 0)
            {
                PropertyField("_meshFilter", "Mesh Filter");
                PropertyField("_meshRenderer", "Mesh Renderer");

                if (_meshTypeCache != 0)
                {
                    _meshTypeCache = 0;
                    _meshFilter.objectReferenceValue = Target.GetComponent<MeshFilter>();
                    _meshRenderer.objectReferenceValue = Target.GetComponent<MeshRenderer>();
                }
            }
            else if (_meshType.enumValueIndex == 1)
            {
                PropertyField("_skinnedMeshRenderer", "Skinned Mesh Renderer");

                if (_meshTypeCache != 1)
                {
                    _meshTypeCache = 1;
                    _skinnedMeshRenderer.objectReferenceValue = Target.GetComponent<SkinnedMeshRenderer>();
                }
            }
            GUI.enabled = true;

            OnMeshEffectsGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnMeshEffectsGUI()
        {

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