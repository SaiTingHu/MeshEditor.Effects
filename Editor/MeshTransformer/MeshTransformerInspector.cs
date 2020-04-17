using UnityEditor;
using UnityEngine;

namespace MeshEditor.Effects
{
    [CustomEditor(typeof(MeshTransformer))]
    internal sealed class MeshTransformerInspector : MeshEffectsBaseInspector<MeshTransformer>
    {
        protected override string HeaderName
        {
            get
            {
                return "MESH TRANSFORMER";
            }
        }

        protected override void OnMeshEffectsGUI()
        {
            base.OnMeshEffectsGUI();

            PropertyField("TargetMesh");

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            PropertyField("TargetMaterials");
            GUILayout.EndHorizontal();
        }
    }
}