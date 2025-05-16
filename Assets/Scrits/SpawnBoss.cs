using Goldmetal.UndeadSurvivor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss: MonoBehaviour
{
    [SerializeField] private GameObject [] prefabEnemigos;
    private List<GameObject> bossCreados;
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

    private int actualizarBoss;
    // Start is called before the first frame update
    void Start()
    {
        tiempoDeSpawn = tiempoEntreSpawns;
        tiempoMaxEnemigos = tiempoEntreMaxEnemigos;
        bossCreados = new List<GameObject>();
        playerMain = GameObject.FindGameObjectWithTag("Player").transform;
        actualizarBoss = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //ºsi el tiempo de spawn es menor al tiempo actual y no estoy esperando spawn
        if (esperandoSpawn)
        {
            actualizarBoss++;        
            tiempoDeSpawn -= Time.deltaTime;
            if (actualizarBoss >= 30)
            {
                GameManager.gameManager.CambiarTiempoBoss(tiempoDeSpawn);
                actualizarBoss = 0;
            }            
             if (tiempoDeSpawn <= 0)
             {
                
                    Spawn(); 
                
                
                
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
       
        if (bossCreados.Count >= maxEnemigos)
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
        if (bossCreados.Count < maxEnemigos)
        {
            Debug.Log("Enemigos activos: " + bossCreados.Count);
            Debug.Log("Instancio enemigo");
            Vector2 spawnCircle = RandomPointInAnnulus(new Vector2 (playerMain.transform.position.x, playerMain.transform.position.y), radioSpawn, radioNOspawn);
            //escojo un enemigo aleatorio
            int enemigoAleatorio = Random.Range(0, prefabEnemigos.Length);
            //instancio el enemigo
            GameObject enemigo = Instantiate(prefabEnemigos[enemigoAleatorio], spawnCircle, Quaternion.identity);
            bossCreados.Add(enemigo);
        }
    }
    public void BossMuertoNigro(GameObject bossMuerto)
    {
        //elimina el enemigo de la lista de enemigos creados
        bossCreados.Remove(bossMuerto);
        Destroy(bossMuerto, 0.5f);
        //si el enemigo muere se elimina de la lista de enemigos creados

        Debug.Log("Enemigos activos: " + enemigosActivos);
    }

    public void BossMuerto()
    {
        enemigosActivos--;
        Debug.Log("Enemigos activos: " + enemigosActivos);
    }
    private void OnDrawGizmos()
    {
        //dibujo los circulos de spawn
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioSpawn);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioNOspawn);
    }
    //Creo un get para el tiempo de spawn y el tiempo entre spawn
    public float GetTiempoDeSpawn()
    {
        return tiempoDeSpawn;
    }
    public float GetTiempoEntreSpawn()
    {
        return tiempoEntreSpawns;
    } 
}

