using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCofres : MonoBehaviour
{
    [SerializeField] private GameObject cofresPrefab;
    [SerializeField] private float tiempoCofre;
    private bool spawnCofre;
    [SerializeField] private float tiempoMaxCofre;
    [SerializeField] private float radioSpawn;
    [SerializeField] private float radioNOspawn;
    [SerializeField] private float tiempoDeDestruccion;
    private Transform playerMain;
    // Start is called before the first frame update
    void Start()
    {
        tiempoCofre = tiempoMaxCofre;
        playerMain = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        tiempoCofre -= Time.deltaTime;
        if (tiempoCofre <= 0)
        {
            spawnCofre = true;
            SpawnDelCofre();
        }
        
    }
    public Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
    {
        //Si origin es 0,0, elijo un punto aleatorio en el circulo
        if (origin == Vector2.zero)
        {
            origin = Random.insideUnitCircle;
            Debug.Log("Esto es: " + origin);
        }
        var randomDirection = (Random.insideUnitCircle * origin).normalized;

        var randomDistance = Random.Range(minRadius, maxRadius);

        var point = origin + randomDirection * randomDistance;

        return point;
    }
    private void SpawnDelCofre()
    {
        if (spawnCofre)
        {
            Vector2 spawnCircle = RandomPointInAnnulus(new Vector2(playerMain.transform.position.x, playerMain.transform.position.y), radioSpawn, radioNOspawn);
            GameObject cofreNuevo = Instantiate(cofresPrefab, spawnCircle, Quaternion.identity);
            //Destruyo el cofre 
            Destroy(cofreNuevo, tiempoDeDestruccion);

        }
        tiempoCofre = tiempoMaxCofre;
        spawnCofre = false;
    }
}
