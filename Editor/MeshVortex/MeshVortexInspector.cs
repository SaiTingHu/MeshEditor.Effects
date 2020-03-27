using UnityEditor;
using UnityEngine;

namespace MeshEditor.Effects
{
    [CustomEditor(typeof(MeshVortex))]
    internal sealed class MeshVortexInspector : MeshEffectsBaseInspector<MeshVortex>
    {
        protected override void OnMeshEffectsGUI()
        {
            base.OnMeshEffectsGUI();

            PropertyField("Gravitation");
            PropertyField("CentrifugalForce");
            PropertyField("Center");
            PropertyField("Radius");
        }

        private void OnSceneGUI()
        {
            Handles.Label(Target.Center, "Vortex Center");
            Target.Center = Handles.PositionHandle(Target.Center, Quaternion.identity);
            Target.Radius = Handles.RadiusHandle(Quaternion.identity, Target.Center, Target.Radius);
        }
    }
}