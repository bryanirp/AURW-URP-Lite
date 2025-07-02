using UnityEngine;
using System.Collections.Generic;

namespace aurw.lite
{
    public enum LiteOceanQuality
    {
        Low,
        High
    }

    [ExecuteAlways]
    [AddComponentMenu("AURW/Lite/Ocean Generation")]
    public class AURW_LiteGeneration : MonoBehaviour
    {
        public AURW_LiteMaterialManager mats;
        public string matName = "Ocean";
        public LiteOceanQuality qualityMat = LiteOceanQuality.High;
        public Vector2 chunkSize = new Vector2(16, 16);
        public int resolution = 8;
        public int chunkRadius = 2;

        private List<Chunk> generatedChunks = new List<Chunk>();

        #region Configuration API

        public AURW_LiteGeneration SetQuality(LiteOceanQuality setQual)
        {
            qualityMat = setQual;
            return this;
        }

        public LiteOceanQuality GetQuality()
        {
            return qualityMat;
        }

        public AURW_LiteGeneration SetMatName(string name)
        {
            matName = name;
            return this;
        }

        public string GetMatName()
        {
            return matName;
        }

        public AURW_LiteGeneration SetMaterialManager(AURW_LiteMaterialManager materialManager)
        {
            mats = materialManager;
            return this;
        }

        public AURW_LiteMaterialManager GetMaterialManager()
        {
            return mats;
        }

        #endregion

        public void Generate()
        {
            DestroyAllChunks(); // Avoid duplicates

            if (mats != null)
                mats.Validate();

            BuildChunks builder = new BuildChunks();
            generatedChunks = builder.GenerateChunks(chunkSize, resolution, chunkRadius);

            Mesh sharedMesh = GenerateFlatPlane(chunkSize, resolution);
            Material matToUse = GetMaterialByQuality();

            foreach (Chunk chunk in generatedChunks)
            {
                GameObject go = new GameObject($"Chunk_{chunk.chunkCoord.x}_{chunk.chunkCoord.y}");

                Vector3 pos = chunk.worldPosition;
                pos.y = transform.position.y;
                go.transform.position = pos;
                go.transform.parent = this.transform;
                go.transform.localScale = Vector3.one;
                go.isStatic = true;

                MeshFilter mf = go.AddComponent<MeshFilter>();
                MeshRenderer mr = go.AddComponent<MeshRenderer>();

                mf.sharedMesh = sharedMesh;

                if (matToUse != null)
                    mr.sharedMaterial = matToUse;
                else
                    Debug.LogWarning($"[AURW] No material applied to chunk at {chunk.chunkCoord}");
            }
        }

        public void DestroyAllChunks()
        {
            List<Transform> toDestroy = new List<Transform>();

            foreach (Transform child in transform)
            {
                if (child.name.StartsWith("Chunk_"))
                    toDestroy.Add(child);
            }

            foreach (Transform child in toDestroy)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(child.gameObject);
                else
                    Destroy(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }

            Debug.Log($"[AURW] {toDestroy.Count} chunks destroyed.");
        }

        Mesh GenerateFlatPlane(Vector2 size, int resolution)
        {
            int vertsX = resolution;
            int vertsZ = resolution;

            Vector3[] verts = new Vector3[vertsX * vertsZ];
            Vector2[] uvs = new Vector2[vertsX * vertsZ];
            int[] tris = new int[(vertsX - 1) * (vertsZ - 1) * 6];

            float stepX = size.x / (vertsX - 1);
            float stepZ = size.y / (vertsZ - 1);

            for (int z = 0; z < vertsZ; z++)
            {
                for (int x = 0; x < vertsX; x++)
                {
                    int i = x + z * vertsX;
                    float vx = x * stepX;
                    float vz = z * stepZ;
                    verts[i] = new Vector3(vx, 0, vz);
                    uvs[i] = new Vector2((float)x / (vertsX - 1), (float)z / (vertsZ - 1));
                }
            }

            int triIndex = 0;
            for (int z = 0; z < vertsZ - 1; z++)
            {
                for (int x = 0; x < vertsX - 1; x++)
                {
                    int i = x + z * vertsX;

                    tris[triIndex++] = i;
                    tris[triIndex++] = i + vertsX;
                    tris[triIndex++] = i + vertsX + 1;

                    tris[triIndex++] = i;
                    tris[triIndex++] = i + vertsX + 1;
                    tris[triIndex++] = i + 1;
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = "GeneratedChunkPlane";
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Center mesh around origin
            Vector3 offset = new Vector3(size.x * 0.5f, 0, size.y * 0.5f);
            for (int i = 0; i < verts.Length; i++)
                verts[i] -= offset;
            mesh.vertices = verts;
            mesh.RecalculateBounds();

            return mesh;
        }

        public Material GetMaterialByQuality()
        {
            if (mats == null || string.IsNullOrEmpty(matName))
            {
                Debug.LogWarning("[AURW] Material manager or material name not set.");
                return null;
            }

            foreach (var couple in mats.liteCouples)
            {
                if (couple != null && couple.waterName == matName)
                {
                    if (qualityMat == LiteOceanQuality.Low)
                        return couple.lowQuality;
                    else if (qualityMat == LiteOceanQuality.High)
                        return couple.highQuality;
                }
            }

            Debug.LogWarning($"[AURW] No material found with name '{matName}'.");
            return null;
        }
    }
}
