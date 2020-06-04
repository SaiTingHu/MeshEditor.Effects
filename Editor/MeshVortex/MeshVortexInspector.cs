using UnityEditor;
using UnityEngine;

namespace MeshEditor.Effects
{
    [CustomEditor(typeof(MeshVortex))]
    internal sealed class MeshVortexInspector : MeshEffectsBaseInspector<MeshVortex>
    {
        protected override string HeaderName
        {
            get
            {
                return "MESH VORTEX";
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

            PropertyField("Gravitation");
            PropertyField("CentripetalForce");
            PropertyField("CentripetalDirection");
            PropertyField("Center");
            PropertyField("Radius");
        }

        private void OnSceneGUI()
        {
            Handles.Label(Target.Center, "Vortex Center");
            Handles.DrawLine(Target.transform.position, Target.Center);
            Target.Center = Handles.PositionHandle(Target.Center, Quaternion.identity);
            Target.Radius = Handles.RadiusHandle(Quaternion.identity, Target.Center, Target.Radius);
        }
    }
}