using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ChunkManagerTiles : MonoBehaviour
{
    [Header("VERSIÓN OPTIMIZADA DEL CHUNKMANAGER")]

    [Header("Configuración de los Chunks")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private int chunkSize = 32;
    [SerializeField] private int viewDistanceChunks = 2;

    [Header("Chunks pre-creados. Minimo = (viewDistanceChunks * 2 + 1)²")]
    [SerializeField] private int maxChunksPool = 25;

    [Header("Actualización de Chunks")]
    [SerializeField] private float updateThreshold = 10f;

    [Header("Numero de chunks que se calculan por fotograma.")]
    [SerializeField] private int chunksPerFrame = 2;

    private Dictionary<Vector2Int, GameObject> activeChunks = new();
    private Queue<GameObject> chunkPool = new();

    private Vector3 lastPlayerPosition;
    private bool isUpdatingChunks = false;

    void OnEnable()
    {
        GameEvents.OnPlayerSpawned += InicializarChunkManager;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerSpawned -= InicializarChunkManager;
    }

    private void InicializarChunkManager(GameObject jugador)
    {
        player = jugador.transform;
        lastPlayerPosition = player.position;

        int recommendedMin = (viewDistanceChunks * 2 + 1) * (viewDistanceChunks * 2 + 1);
        if (maxChunksPool < recommendedMin)
        {
            Debug.LogWarning($"El valor de maxChunksPool ({maxChunksPool}) es menor que el recomendado ({recommendedMin}). Se ajustará al mínimo recomendado.");
            maxChunksPool = recommendedMin;
        }

        for (int i = 0; i < maxChunksPool; i++)
        {
            GameObject newChunk = Instantiate(chunkPrefab);
            newChunk.SetActive(false);
            chunkPool.Enqueue(newChunk);
        }

        StartCoroutine(CheckAndUpdateChunksCoroutine(force: true));
    }

    void Update()
    {
        if (player == null || isUpdatingChunks) return;

        if (Vector3.Distance(player.position, lastPlayerPosition) > updateThreshold)
        {
            StartCoroutine(CheckAndUpdateChunksCoroutine());
            lastPlayerPosition = player.position;
        }
    }

    IEnumerator CheckAndUpdateChunksCoroutine(bool force = false)
    {
        isUpdatingChunks = true;

        if (player == null)
        {
            isUpdatingChunks = false;
            yield break;
        }

        Vector2Int playerChunkCoord = WorldToChunkCoord(player.position);
        int count = 0;

        HashSet<Vector2Int> newVisibleChunks = new();

        for (int x = -viewDistanceChunks; x <= viewDistanceChunks; x++)
        {
            for (int y = -viewDistanceChunks; y <= viewDistanceChunks; y++)
            {
                Vector2Int coord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + y);
                newVisibleChunks.Add(coord);

                if (!activeChunks.ContainsKey(coord))
                {
                    GameObject chunkObject;
                    if (chunkPool.Count > 0)
                    {
                        chunkObject = chunkPool.Dequeue();
                        chunkObject.SetActive(true);
                        chunkObject.transform.position = new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0);
                        chunkObject.GetComponent<ChunkTilemap>()?.Initialize(coord, chunkSize);
                    }
                    else
                    {
                        chunkObject = Instantiate(chunkPrefab, new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0), Quaternion.identity);
                        chunkObject.name = $"Chunk_{coord.x}_{coord.y}";
                        chunkObject.GetComponent<ChunkTilemap>()?.Initialize(coord, chunkSize);
                    }
                    activeChunks.Add(coord, chunkObject);
                    count++;
                    if (!force && count % chunksPerFrame == 0)
                        yield return null;
                }
            }
        }

        List<Vector2Int> chunksToRemove = new();
        foreach (var kvp in activeChunks)
        {
            if (!newVisibleChunks.Contains(kvp.Key))
            {
                kvp.Value.SetActive(false);
                chunkPool.Enqueue(kvp.Value);
                chunksToRemove.Add(kvp.Key);
            }
        }
        foreach (var coord in chunksToRemove)
        {
            activeChunks.Remove(coord);
        }

        isUpdatingChunks = false;
        yield return null;
    }

    Vector2Int WorldToChunkCoord(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int y = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(x, y);
    }
}
