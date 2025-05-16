using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject[] tiles;

    public float noiseScale = 10f;

    public void Initialize(Vector2Int chunkCoord, int chunkSize)
    {
        Vector2 baseCoord = chunkCoord * chunkSize;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                // Coordenadas globales
                float worldX = baseCoord.x + x;
                float worldY = baseCoord.y + y;

                float noiseValue = Mathf.PerlinNoise(worldX / noiseScale, worldY / noiseScale);

                GameObject tileToSpawn;
                //convierto el valor PerlinNoise 0-1 a un valor entre 0 y tiles.lenght
                int tileIndex = Mathf.FloorToInt(noiseValue * tiles.Length);
                // Asegurarse de que el índice esté dentro del rango
                tileIndex = Mathf.Clamp(tileIndex, 0, tiles.Length - 1);
                //asigno tileToSpawn
                tileToSpawn = tiles[tileIndex];

                Vector3 position = new Vector3(worldX, worldY, 0);
                Instantiate(tileToSpawn, position, Quaternion.identity, transform);
            }
        }
    }
}