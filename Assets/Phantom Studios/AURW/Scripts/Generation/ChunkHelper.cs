using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace aurw.lite
{
    // Chunk Helper
    public class Chunk
    {
        public Vector2Int chunkCoord;
        public Vector3 worldPosition;
        public Vector2 size;
        public int resolution = 1;

        public Chunk SetSize(Vector2 _size)
        {
            if (_size.x > 0 && _size.y > 0)
            {
                size = _size;
            }
            else
            {
                size = new Vector2(1, 1);
                Debug.Log("Invalid chunk size, setting it at default value (1,1)");
            }
            return this;
        }

        public Chunk SetResolution(int _resolution)
        {
            resolution = Mathf.Max(1, _resolution);
            return this;
        }

        public Vector2Int GetChunkCoordinates(Vector3 position)
        {
            int chunkX = Mathf.FloorToInt(position.x / size.x);
            int chunkY = Mathf.FloorToInt(position.z / size.y);
            return new Vector2Int(chunkX, chunkY);
        }

        public Vector3 GetChunkWorldPosition(Vector2Int chunkCoordinates)
        {
            float x = chunkCoordinates.x * size.x + size.x * 0.5f;
            float z = chunkCoordinates.y * size.y + size.y * 0.5f;
            return new Vector3(x, 0, z);
        }

        public Vector2 GetLocalPositionInChunk(Vector3 worldPosition)
        {
            Vector2Int chunkCoords = GetChunkCoordinates(worldPosition);
            float localX = (worldPosition.x - chunkCoords.x * size.x) / size.x;
            float localY = (worldPosition.z - chunkCoords.y * size.y) / size.y;
            return new Vector2(localX, localY);
        }
        public Bounds GetChunkBounds(Vector2Int chunkCoordinates)
        {
            Vector3 center = new Vector3(
                chunkCoordinates.x * size.x + size.x * 0.5f,
                0,
                chunkCoordinates.y * size.y + size.y * 0.5f);

            Vector3 size3D = new Vector3(size.x, 0, size.y);
            return new Bounds(center, size3D);
        }

        public float GetResolutionPerUnit()
        {
            return resolution / Mathf.Max(size.x, size.y);
        }
    }

    public class BuildChunks : Chunk
    {
        public List<Chunk> GenerateChunks(Vector2 size, int resolution, int chunkRadius)
        {
            List<Chunk> chunks = new List<Chunk>();

            SetSize(size);
            SetResolution(resolution);

            for (int y = -chunkRadius; y <= chunkRadius; y++)
            {
                for (int x = -chunkRadius; x <= chunkRadius; x++)
                {
                    Chunk c = new Chunk()
                        .SetSize(size)
                        .SetResolution(resolution);

                    Vector2Int coord = new Vector2Int(x, y);
                    c.chunkCoord = coord;
                    c.worldPosition = c.GetChunkWorldPosition(coord);

                    chunks.Add(c);
                }
            }

            return chunks;
        }
        public void BakeQualityScene()
        {
            // Not useful for now
            // The idea is fixing coast rendering. 
        }
    }
}


