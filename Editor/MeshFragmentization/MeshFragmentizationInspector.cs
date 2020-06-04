using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MeshEditor.Effects
{
    [CustomEditor(typeof(MeshFragmentization))]
    internal sealed class MeshFragmentizationInspector : MeshEffectsBaseInspector<MeshFragmentization>
    {
        protected override string HeaderName
        {
            get
            {
                return "MESH FRAGMENTIZATION";
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Tools.current = Tool.View;
        }

        protected override void OnMeshEffectsGUI()
        {
            base.OnMeshEffectsGUI();

            PropertyField("FragPoint");
            PropertyField("FragHealth");
            PropertyField("FragSpeed");
            PropertyField("FragRate");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Fragment Type");
            if (GUILayout.Button(Target.FragmentType, "MiniPopup"))
            {
                GenericMenu gm = new GenericMenu();
                List<Type> types = Toolkit.GetTypesInRunTimeAssemblies();
                for (int i = 0; i < types.Count; i++)
                {
                    if (types[i] == typeof(FragmentBehaviour) || types[i].IsSubclassOf(typeof(FragmentBehaviour)))
                    {
                        int j = i;
                        gm.AddItem(new GUIContent(types[j].FullName), Target.FragmentType == types[j].FullName, () =>
                        {
                            Undo.RecordObject(target, "Set FragmentType");
                            Target.FragmentType = types[j].FullName;
                            HasChanged();
                        });
                    }
                }
                gm.ShowAsContext();
            }
            GUILayout.EndHorizontal();
        }

        private void OnSceneGUI()
        {
            Handles.Label(Target.FragPoint, "Frag Point");
            Handles.DrawLine(Target.transform.position, Target.FragPoint);
            Target.FragPoint = Handles.PositionHandle(Target.FragPoint, Quaternion.identity);
        }
    }
}