using UnityEditor;

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
        }
    }
}