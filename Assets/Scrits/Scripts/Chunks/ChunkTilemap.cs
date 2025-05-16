using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ChunkTilemap : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap tilemap;         // Referencia al componente Tilemap. Si no se asigna en el editor, se buscará en Awake.
    public TileBase[] tiles;        // Array de tiles para pintar el chunk
    public float noiseScale = 10f;  // Escala para el cálculo del Perlin Noise

    [Header("Decoración Opcional")]
    [Tooltip("Lista de prefabs decorativos (arbustos, árboles, estatuas, etc.)")]
    public List<GameObject> decorPrefabs;
    [Tooltip("Número mínimo de decorativos a colocar en el chunk")]
    public int minDecorCount = 1;
    [Tooltip("Número máximo de decorativos a colocar en el chunk")]
    public int maxDecorCount = 5;
    [Tooltip("Espacio mínimo (en número de tiles) entre decorativos")]
    public int decorSpacing = 2;
    [Tooltip("Probabilidad (0 a 1) de que se intente colocar un decorativo una vez superado el espacio mínimo")]
    public float probSpawnObject = 0.5f;

    public List<GameObject> decorationTiles = new(); //Array donde guardamos los decorativos que se colocan en el chunk
    public void Initialize(Vector2Int chunkCoord, int chunkSize)
    {
        // Si no se asignó el Tilemap, se busca en el mismo GameObject
        if (tilemap == null)
            tilemap = GetComponent<Tilemap>();

        // Limpiar el Tilemap para evitar solapamientos
        tilemap.ClearAllTiles();

        // Limpiar la lista de decorativos
        LimpiarDecoracion();

        // Calcular la posición base global de este chunk
        Vector2 baseCoord = chunkCoord * chunkSize;

        // Calcular un número aleatorio de decorativos a colocar en este chunk dentro del rango
        int decorTarget = Random.Range(minDecorCount, maxDecorCount + 1);
        int decorPlaced = 0;

        // Contador que acumula la cantidad de tiles recorridos desde la última colocación de decorativo.
        int tilesSinceLastDecor = 0;

        // Recorre cada celda del chunk
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                // Calcular las coordenadas globales para obtener el valor de Perlin Noise.
                float worldX = baseCoord.x + x;
                float worldY = baseCoord.y + y;
                float noiseValue = Mathf.PerlinNoise(worldX / noiseScale, worldY / noiseScale);

                int tileIndex = Mathf.FloorToInt(noiseValue * tiles.Length);
                tileIndex = Mathf.Clamp(tileIndex, 0, tiles.Length - 1);
                TileBase tile = tiles[tileIndex];

                // Colocar el tile en el Tilemap usando la posición de celda (cellPosition)
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(cellPosition, tile);

                // Obtener la posición central de la celda
                Vector3 cellCenterPos = tilemap.GetCellCenterWorld(cellPosition);

                // Comprobar si se debe intentar colocar un decorativo
                if (tilesSinceLastDecor >= decorSpacing && decorPlaced < decorTarget && decorPrefabs.Count > 0)
                {
                    if (Random.value <= probSpawnObject)
                    {
                        // Seleccionar aleatoriamente un prefab decorativo
                        GameObject decorPrefab = decorPrefabs[Random.Range(0, decorPrefabs.Count)];
                        // Instanciar el decorativo en el centro de la celda
                        GameObject aux = Instantiate(decorPrefab, cellCenterPos, Quaternion.identity, transform);
                        //Guardar el decorativo en el array
                        decorationTiles.Add(aux);
                        Debug.Log($"Decorativo colocado en: {cellCenterPos}");
                        decorPlaced++;
                        // Reiniciar el contador de separación
                        tilesSinceLastDecor = 0;
                    }
                }

                // Incrementar el contador de tiles desde el último decorativo
                tilesSinceLastDecor++;
            }
        }
    }
    public void LimpiarDecoracion()
    {
        //Eliminamos los objetos decorativos antiguos y vaciamos la lista
        foreach (GameObject decor in decorationTiles)
        {
            Destroy(decor);
        }
        decorationTiles.Clear();
    }
}