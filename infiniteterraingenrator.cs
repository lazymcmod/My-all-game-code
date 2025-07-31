using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrainGenerator : MonoBehaviour
{
    public Transform player;
    public int chunkSize = 16;
    public int renderDistance = 3;
    public int chunkHeight = 6;

    public GameObject bedrockPrefab;
    public GameObject deepslatePrefab;
    public GameObject stonePrefab;
    public GameObject sandPrefab;
    public GameObject dirtPrefab;
    public GameObject grassblockPrefab;

    public GameObject logPrefab;
    public GameObject leafPrefab;
    public float treeChance = 0.01f;

    public GameObject waterPrefab; // ✅ Water prefab
    public int waterLevel = 10;    // ✅ Water level

    public float caveNoiseScale = 0.08f;
    public float caveThreshold = 0.5f;

    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();

    void Update()
    {
        if (player == null) return; // ✅ Fix: avoid crash if player destroyed or missing

        Vector2Int playerChunkCoord = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.z / chunkSize)
        );

        HashSet<Vector2Int> chunksToKeep = new HashSet<Vector2Int>();

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + z);
                chunksToKeep.Add(chunkCoord);

                if (!loadedChunks.ContainsKey(chunkCoord))
                {
                    GameObject chunk = GenerateChunk(chunkCoord);
                    loadedChunks.Add(chunkCoord, chunk);
                }
            }
        }

        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunkCoord in loadedChunks.Keys)
        {
            if (!chunksToKeep.Contains(chunkCoord))
            {
                Destroy(loadedChunks[chunkCoord]);
                chunksToRemove.Add(chunkCoord);
            }
        }

        foreach (var coord in chunksToRemove)
        {
            loadedChunks.Remove(coord);
        }
    }

    GameObject GenerateChunk(Vector2Int chunkCoord)
    {
        GameObject chunkParent = new GameObject($"Chunk_{chunkCoord.x}_{chunkCoord.y}");
        chunkParent.transform.position = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                int worldX = chunkCoord.x * chunkSize + x;
                int worldZ = chunkCoord.y * chunkSize + z;

                float terrainNoiseScale = 0.1f;
                int maxTerrainHeight = chunkHeight + 5;
                int height = Mathf.FloorToInt(Mathf.PerlinNoise(worldX * terrainNoiseScale, worldZ * terrainNoiseScale) * maxTerrainHeight);

                bool placedSurface = false;
                bool hasTopBlock = false;

                for (int y = 0; y <= height; y++)
                {
                    float caveX = worldX * caveNoiseScale;
                    float caveY = y * caveNoiseScale;
                    float caveZ = worldZ * caveNoiseScale;
                    float caveNoise = Mathf.PerlinNoise(caveX, caveY) * Mathf.PerlinNoise(caveY, caveZ);

                    if (y > 2 && y < height - 1 && caveNoise > caveThreshold)
                        continue;

                    GameObject blockToPlace = null;

                    if (y == 0)
                        blockToPlace = bedrockPrefab;
                    else if (y <= height - 4)
                        blockToPlace = deepslatePrefab;
                    else if (y <= height - 2)
                        blockToPlace = stonePrefab;
                    else if (y == height - 1)
                        blockToPlace = sandPrefab;
                    else if (!placedSurface)
                    {
                        blockToPlace = grassblockPrefab;
                        placedSurface = true;
                        hasTopBlock = true;

                        if (Random.value < treeChance)
                        {
                            GenerateTree(worldX, y + 1, worldZ, chunkParent.transform);
                        }
                    }

                    if (blockToPlace != null)
                    {
                        Vector3 blockPos = new Vector3(worldX, y, worldZ);
                        Instantiate(blockToPlace, blockPos, Quaternion.identity, chunkParent.transform);
                    }
                }

                // 🌊 Only place water if there's no top block and height < water level
                if (!hasTopBlock && height < waterLevel)
                {
                    for (int y = height + 1; y <= waterLevel; y++)
                    {
                        Vector3 waterPos = new Vector3(worldX, y, worldZ);
                        Instantiate(waterPrefab, waterPos, Quaternion.identity, chunkParent.transform);
                    }
                }
            }
        }

        return chunkParent;
    }

    void GenerateTree(int x, int y, int z, Transform parent)
    {
        int trunkHeight = Random.Range(4, 6);

        for (int i = 0; i < trunkHeight; i++)
        {
            Vector3 trunkPos = new Vector3(x, y + i, z);
            Instantiate(logPrefab, trunkPos, Quaternion.identity, parent);
        }

        int leafStartY = y + trunkHeight - 1;
        for (int lx = -2; lx <= 2; lx++)
        {
            for (int ly = 0; ly <= 2; ly++)
            {
                for (int lz = -2; lz <= 2; lz++)
                {
                    if (Mathf.Abs(lx) == 0 && Mathf.Abs(lz) == 0 && ly < 2)
                        continue;

                    Vector3 leafPos = new Vector3(x + lx, leafStartY + ly, z + lz);
                    Instantiate(leafPrefab, leafPos, Quaternion.identity, parent);
                }
            }
        }
    }
}
