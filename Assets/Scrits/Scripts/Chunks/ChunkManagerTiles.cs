using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ChunkManagerTiles : MonoBehaviour
{
    [Header("VERSIÓN OPTIMIZADA DEL CHUNKMANAGER")]

    //Version optimizada 
    [Header("Configuración de los Chunks")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private int chunkSize = 32;          // Tamaño del chunk (igual que antes)
    [SerializeField] private int viewDistanceChunks = 2;  // Distancia de vision (igual que antes tambien)

    [Header("Chunks pre-creados. Minimo = (viewDistanceChunks * 2 + 1)²")]
    [SerializeField] private int maxChunksPool = 25;     

    [Header("Actualización de Chunks")]
    [SerializeField] private float updateThreshold = 10f; // MEJORA: Ahora las actualizaciones se hacen en base a la distancia

    [Header("Numero de chunks que se calculan por fotograma.")]
    [SerializeField] private int chunksPerFrame = 2; // Número de chunks que se procesan por frame para evitar congelaciones en el juego. Se puede ajustar según el rendimiento deseado.]

    // Diccionario de chunks procedurales actualmente activos y su posicion (0,1), (2,4), etc.
    private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();
    
    // Cola de chunks procedurales que se reutilizan (pool)
    private Queue<GameObject> chunkPool = new();

    private Vector3 lastPlayerPosition;

    void Start()
    {
        //Asigno al player
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        lastPlayerPosition = player.position;

        // Por si ponemos un numero insuficiente de chunks
        int recommendedMin = (viewDistanceChunks * 2 + 1) * (viewDistanceChunks * 2 + 1);
        if (maxChunksPool < recommendedMin)
        {
            Debug.LogWarning($"El valor de maxChunksPool ({maxChunksPool}) es menor que el recomendado ({recommendedMin}). Se ajustará al mínimo recomendado.");
            maxChunksPool = recommendedMin;
        }

        // Precargar el pool de chunks procedurales
        for (int i = 0; i < maxChunksPool; i++)
        {
            GameObject newChunk = Instantiate(chunkPrefab);
            //newChunk.GetComponent<ChunkTilemap>()?.Initialize(new Vector2Int(), chunkSize); //Por si quisiéramos una optimización aun mayor.
            newChunk.SetActive(false);
            chunkPool.Enqueue(newChunk);
        }

        // Los chunks personalizados NO ESTAN IMPLEMENTADOS, ahí cada uno los coloca segun su diseño

        StartCoroutine(CheckAndUpdateChunksCoroutine());
    }

    void Update()
    {
        // Sólo se actualiza cuando el jugador se mueve más que el umbral definido.
        if (Vector3.Distance(player.position, lastPlayerPosition) > updateThreshold)
        {
            StartCoroutine(CheckAndUpdateChunksCoroutine());
            lastPlayerPosition = player.position;
        }
    }

    IEnumerator CheckAndUpdateChunksCoroutine()
    {
        Vector2Int playerChunkCoord = WorldToChunkCoord(player.position); // Convertir la posición del jugador a coordenadas de chunk.
        int count = 0;

        // Generar (o reubicar) los chunks procedurales que deban estar activos en el rango.
        // No deberían generarse si el numero de chunks está bien creado
        for (int x = -viewDistanceChunks; x <= viewDistanceChunks; x++)
        {
            for (int y = -viewDistanceChunks; y <= viewDistanceChunks; y++)
            {
                Vector2Int coord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + y);
                // Si ya existe un chunk activo en esa posición, se omite.
                if (!activeChunks.ContainsKey(coord))
                {
                    GameObject chunkObject;
                    if (chunkPool.Count > 0)
                    {
                        // Reutilizar un chunk del pool.
                        chunkObject = chunkPool.Dequeue();
                        chunkObject.SetActive(true);
                        chunkObject.transform.position = new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0);
                        chunkObject.GetComponent<ChunkTilemap>()?.Initialize(coord, chunkSize); //Comentar y ver linea 48.
                    }
                    else
                    {
                        // Como respaldo, instanciar si el pool se agotara (lo ideal es tener suficiente pool).
                        chunkObject = Instantiate(chunkPrefab, new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0), Quaternion.identity);
                        chunkObject.name = $"Chunk_{coord.x}_{coord.y}";
                        chunkObject.GetComponent<ChunkTilemap>()?.Initialize(coord, chunkSize);
                    }
                    activeChunks.Add(coord, chunkObject);
                    count++;
                    // Distribuir la carga en varios frames si se crean muchos chunks.
                    if (count % chunksPerFrame == 0)
                        yield return null;
                }
            }
        }

        // Detectar y desactivar chunks procedurales que ya no están en el área de interés.
        List<Vector2Int> chunksToRemove = new();
        foreach (var kvp in activeChunks)
        {
            // Se comprueba la distancia en términos de coordenadas de chunk.
            if (Vector2Int.Distance(playerChunkCoord, kvp.Key) > viewDistanceChunks)
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

        yield return null;
    }

    // Convierte una posición del mundo a coordenadas de chunk.
    Vector2Int WorldToChunkCoord(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int y = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(x, y);
    }
}