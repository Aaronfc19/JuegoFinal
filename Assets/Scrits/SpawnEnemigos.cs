using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemigos : MonoBehaviour
{
    [SerializeField] private GameObject [] prefabEnemigos;
    [SerializeField] private float tiempoEntreSpawns;
    [SerializeField] private int maxEnemigos;
    [SerializeField] private int enemigosActivos;
    [SerializeField] private float radioSpawn;
    [SerializeField] private float radioNOspawn;
    [SerializeField] private float tiempoDeSpawn;
    [SerializeField] private bool esperandoSpawn;
    private Transform playerMain;
    private float tiempoMaxEnemigos;
    [SerializeField] private float tiempoEntreMaxEnemigos;
    private bool enemigosMaximos;
    // Start is called before the first frame update
    void Start()
    {
        tiempoDeSpawn = tiempoEntreSpawns;
        tiempoMaxEnemigos = tiempoEntreMaxEnemigos;
        playerMain = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //ºsi el tiempo de spawn es menor al tiempo actual y no estoy esperando spawn
        if (esperandoSpawn)
        {
        
            tiempoDeSpawn -= Time.deltaTime;
             if (tiempoDeSpawn <= 0)
             {
                if (GameManager.gameManager.GetMinutos() <= 1)
                {
                    SpawnEnemigos0();
                }
                if (GameManager.gameManager.GetMinutos() >= 1)
                {
                    Spawn();
                }
                
                tiempoDeSpawn = tiempoEntreSpawns;
            }   
        }
        if (enemigosMaximos)
        {
            tiempoMaxEnemigos -= Time.deltaTime;
           
            AumentarMaxEnemigos();
            
            tiempoMaxEnemigos = tiempoEntreMaxEnemigos;
        }
        if (tiempoMaxEnemigos <= 0)
        {
            enemigosMaximos = true;
        }
        else
        {
            enemigosMaximos = false;
        }

        {
            tiempoMaxEnemigos -= Time.deltaTime;
        }
       
        if (enemigosActivos >= maxEnemigos)
        {
            esperandoSpawn = false;
        }
        else
        {
            esperandoSpawn = true;
        }


    }
    //Cada 10 segundos MaxEnemigos aumenta en 1
    public void AumentarMaxEnemigos()
    {
        
            maxEnemigos++;
            Debug.Log("Maximo de enemigos: " + maxEnemigos);
        
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
    private void Spawn()
    {
        if (enemigosActivos < maxEnemigos)
        {
            Debug.Log("Enemigos activos: " + enemigosActivos);
            Debug.Log("Instancio enemigo");
            Vector2 spawnCircle = RandomPointInAnnulus(new Vector2 (playerMain.transform.position.x, playerMain.transform.position.y), radioSpawn, radioNOspawn);
            //escojo un enemigo aleatorio
            int enemigoAleatorio = Random.Range(0, prefabEnemigos.Length);
            //instancio el enemigo
            GameObject enemigo = Instantiate(prefabEnemigos[enemigoAleatorio], spawnCircle, Quaternion.identity);
            enemigosActivos++;
        }
    }
    //Si el TimepoCronometro es menor de 01:00 solo se spawnean enemigos de la lista 0
    private void SpawnEnemigos0()
    {
        if (enemigosActivos < maxEnemigos)
        {
            Debug.Log("Enemigos activos: " + enemigosActivos);
            Debug.Log("Instancio enemigo");
            Vector2 spawnCircle = RandomPointInAnnulus(new Vector2(playerMain.transform.position.x, playerMain.transform.position.y), radioSpawn, radioNOspawn);
            //escojo un enemigo aleatorio
            int enemigoInicial = 0;
            //instancio el enemigo
            GameObject enemigo = Instantiate(prefabEnemigos[enemigoInicial], spawnCircle, Quaternion.identity);
            enemigosActivos++;
        }
    }
    public void EnemigoMuerto()
    {
        enemigosActivos--;
        Debug.Log("Enemigos activos: " + enemigosActivos);
    }
    public void EsperaTiempo() 
    {
       
    }
    private void OnDrawGizmos()
    {
        //dibujo los circulos de spawn
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioSpawn);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioNOspawn);
    }
}

