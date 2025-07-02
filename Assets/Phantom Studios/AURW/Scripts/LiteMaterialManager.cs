using UnityEngine;
using System.Linq;
using UnityEditor;

namespace aurw.lite
{
    [CreateAssetMenu(fileName = "LiteMaterialManager", menuName = "AURW/Lite/MaterialManager")]
    public class AURW_LiteMaterialManager : ScriptableObject
    {
        public AURW_LiteMaterials[] liteCouples;

        public void Validate()
        {
            if (liteCouples == null || liteCouples.Length == 0)
            {
                Debug.LogWarning("[AURW] No material couples defined.");
                return;
            }

            for (int i = 0; i < liteCouples.Length; i++)
            {
                var couple = liteCouples[i];
                if (couple.lowQuality == null)
                {
                    string name = string.IsNullOrEmpty(couple.waterName) ? $"Element #{i}" : couple.waterName;
                    Debug.LogError($"[AURW] Missing required low quality material in '{name}'");
#if UNITY_EDITOR
                    EditorApplication.isPaused = true;
#endif
                }
            }
        }

        [System.Serializable]
        public class AURW_LiteMaterials
        {
            public string waterName;
            public Material lowQuality; // Required
            public Material highQuality; // Optional
        }
    }
}