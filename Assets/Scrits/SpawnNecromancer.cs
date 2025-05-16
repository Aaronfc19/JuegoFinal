using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnNecromancer : MonoBehaviour
{
    [SerializeField] private GameObject [] prefabEnemigos;
     private List <GameObject> enemigosCreados;
    [SerializeField] private float tiempoEntreSpawns;
    [SerializeField] private int maxEnemigos;
    [SerializeField] private int enemigosActivos;
    [SerializeField] private float radioSpawn;
    [SerializeField] private float radioNOspawn;
    [SerializeField] private float tiempoDeSpawn;
    [SerializeField] private bool esperandoSpawn;
    [SerializeField] private GameObject playerMain;
    [SerializeField] private Animator animator;
    public bool spawnEnemigos;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        tiempoDeSpawn = tiempoEntreSpawns;
        enemigosCreados = new List<GameObject>();
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
                if (GameManager.gameManager.GetMinutos() < 1)
                {
                    Debug.Log("Estoy en esta");
                    spawnEnemigos = true;
                    SpawnEnemigos0();
                    
                }
                if (GameManager.gameManager.GetMinutos() >= 1)
                {
                    Debug.Log("Estoy en esta 2");
                    spawnEnemigos = true;
                    Spawn();
                    
                }
                
                tiempoDeSpawn = tiempoEntreSpawns;
            }
        }
        if (enemigosCreados.Count >= maxEnemigos)
        {
            esperandoSpawn = false;
        }
        else
        {
            esperandoSpawn = true;
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
    public void Spawn()
    {
        if (enemigosCreados.Count < maxEnemigos)
        {
            
            //escojo un enemigo aleatorio
            int enemigoAleatorio = Random.Range(0, prefabEnemigos.Length);
            //uso un bucle for para instanciar maxEnemigos enemigos
            for (int i = 0; i < maxEnemigos; i++)
            { 
                 Vector2 spawnCircle = RandomPointInAnnulus(new Vector2 (playerMain.transform.position.x, playerMain.transform.position.y), radioSpawn, radioNOspawn);
                GameObject enemigo = Instantiate(prefabEnemigos[enemigoAleatorio], spawnCircle, Quaternion.identity);
                //le paso el script del spawn al enemigo
                enemigo.GetComponent<EnemigoGeneradoPorNigro>().ScriptNecromancer(this);
                //guardo al enemigo en la lista de enemigos creados
                enemigosCreados.Add(enemigo);
                
                Debug.Log("Instancio enemigo: " + prefabEnemigos[enemigoAleatorio]);
            }
            
        }
    }
    //Si el TimepoCronometro es menor de 01:00 solo se spawnean enemigos de la lista 0
    public void SpawnEnemigos0()
    {
        if (enemigosCreados.Count < maxEnemigos)
        {
            Debug.Log("Enemigos activos: " + enemigosActivos);
            Debug.Log("Instancio enemigo");
            for (int i = 0; i < maxEnemigos; i++)
            {
                Vector2 spawnCircle = RandomPointInAnnulus(new Vector2(playerMain.transform.position.x, playerMain.transform.position.y), radioSpawn, radioNOspawn);
                //escojo un enemigo aleatorio
                int enemigoInicial = 0;
                //instancio el enemigo
                GameObject enemigo = Instantiate(prefabEnemigos[enemigoInicial], spawnCircle, Quaternion.identity);
                enemigo.GetComponent<EnemigoGeneradoPorNigro>().ScriptNecromancer(this);
                //guardo al enemigo en la lista de enemigos creados
                enemigosCreados.Add(enemigo);
                
            }
        }
    }
    public void EnemigoMuertoNigro(GameObject enemigoMuerto)
    {
        //elimina el enemigo de la lista de enemigos creados
        enemigosCreados.Remove(enemigoMuerto);
        Destroy(enemigoMuerto, 0.5f);
        //si el enemigo muere se elimina de la lista de enemigos creados
       
        Debug.Log("Enemigos activos: " + enemigosActivos);
    }
    public void DestruirTodos() 
    {
        //destruye todos los enemigos creados
        foreach (GameObject enemigo in enemigosCreados)
        {
            Destroy(enemigo);
        }
        enemigosCreados.Clear();
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

