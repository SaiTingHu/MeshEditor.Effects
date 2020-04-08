using UnityEditor;

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

        protected override void OnMeshEffectsGUI()
        {
            base.OnMeshEffectsGUI();

            PropertyField("WindPower");
            PropertyField("WindPowerMinRate");
            PropertyField("WindPowerMaxRate");
        }
    }
}