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

        protected override void OnMeshEffectsGUI()
        {
            base.OnMeshEffectsGUI();

            PropertyField("FragPoint");
            PropertyField("IntervalTime");
        }

        private void OnSceneGUI()
        {
            Handles.Label(Target.FragPoint, "Frag Point");
            Handles.DrawLine(Target.transform.position, Target.FragPoint);
            Target.FragPoint = Handles.PositionHandle(Target.FragPoint, Quaternion.identity);
        }
    }
}