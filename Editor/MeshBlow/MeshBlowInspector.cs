using UnityEditor;
using UnityEngine;

namespace MeshEditor.Effects
{
    [CustomEditor(typeof(MeshBlow))]
    internal sealed class MeshBlowInspector : MeshEffectsBaseInspector<MeshBlow>
    {
        protected override string HeaderName
        {
            get
            {
                return "MESH BLOW";
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

            PropertyField("WindPower");
            PropertyField("WindPowerMinRate");
            PropertyField("WindPowerMaxRate");
        }

        private void OnSceneGUI()
        {
            using (new Handles.DrawingScope(Color.green))
            {
                Handles.Slider(Target.transform.position, Target.BlowsDirection);
            }
        }
    }
}