using UnityEditor;
using UnityEngine;
namespace aurw.lite
{
    [CustomEditor(typeof(AURW_LiteGeneration))]
    [CanEditMultipleObjects]
    public class AURW_LiteGeneration_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            AURW_LiteGeneration pc = (AURW_LiteGeneration)target;
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate"))
            {
                pc.Generate();
            }
            if (GUILayout.Button("Reset"))
            {
                pc.DestroyAllChunks();
            }
        }
    }
}