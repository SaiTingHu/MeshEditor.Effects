using UnityEditor;

namespace MeshEditor.Effects
{
    [CustomEditor(typeof(MeshWave))]
    internal sealed class MeshWaveInspector : MeshEffectsBaseInspector<MeshWave>
    {
        protected override string HeaderName
        {
            get
            {
                return "MESH WAVE";
            }
        }

        protected override void OnMeshEffectsGUI()
        {
            base.OnMeshEffectsGUI();

            PropertyField("Direction");
            PropertyField("IsFlat");
            PropertyField("WavePower");
            PropertyField("WaveSpeed");
        }
    }
}