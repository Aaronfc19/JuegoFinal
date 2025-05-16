using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Transform player;
    public int chunkSize = 16;
    public int viewDistance = 3; // en chunks

    private Dictionary<Vector2Int, GameObject> chunks = new Dictionary<Vector2Int, GameObject>();

    void Update()
    {
        Vector2Int playerChunkCoord = WorldToChunkCoord(player.position);

        // Generar nuevos chunks cerca del jugador
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int coord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + y);
                if (!chunks.ContainsKey(coord))
                {
                    GenerateChunk(coord);
                }
            }
        }

        // Eliminar chunks lejanos
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var kvp in chunks)
        {
            if (Vector2Int.Distance(playerChunkCoord, kvp.Key) > viewDistance)
            {
                Destroy(kvp.Value);
                chunksToRemove.Add(kvp.Key);
            }
        }

        foreach (var coord in chunksToRemove)
        {
            chunks.Remove(coord);
        }
    }

    Vector2Int WorldToChunkCoord(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int y = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(x, y);
    }

    void GenerateChunk(Vector2Int coord)
    {
        Vector3 worldPosition = new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0);
        GameObject newChunk = Instantiate(chunkPrefab, worldPosition, Quaternion.identity);
        newChunk.name = $"Chunk_{coord.x}_{coord.y}";
        newChunk.GetComponent<Chunk>().Initialize(coord, chunkSize);
        chunks.Add(coord, newChunk);
    }
}