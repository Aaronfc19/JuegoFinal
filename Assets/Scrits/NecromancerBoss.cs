using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBoss : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private float vidaEnemigo;
    [SerializeField] private float danoRecibido;
    [SerializeField] private float tiempoKnockUP;
    [SerializeField] private float fuerzaDeEmpuje;
    [SerializeField] private GameObject[] invocacionesNigromante;
    [SerializeField] private int puntos;
    //Para las particulas de muerte de los enemigos
    [SerializeField] GameObject boom;
    [SerializeField] AudioSource audioSourceEnemigos;
    [SerializeField] private List<AudioClip> audiosEnemigos;
    [SerializeField] private ParticleSystem particulasDead;
    private SpawnNecromancer spawnBoss;
    private bool knokeadoSTOP;
    private bool muertoEnemy;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        muertoEnemy = false;
        knokeadoSTOP = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Muevo al enemigo hacia el jugador
        if (knokeadoSTOP)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        //Hago que el enemigo mire hacia la dirección en la que se mueve
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (transform.position.x > player.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (GetComponent<SpawnNecromancer>().spawnEnemigos)
        {
            animator.SetTrigger("InvokeCool");
            GetComponent<SpawnNecromancer>().spawnEnemigos = false;
        }
    }
    public void RecibirDanyoNecro(Vector2 empujeBala, float danoRecibido)
    {
        vidaEnemigo -= danoRecibido;
        if (vidaEnemigo <= 0 && !muertoEnemy)
        {
            FindAnyObjectByType<SpawnBoss>().BossMuertoNigro(this.gameObject);
            ScoreManager.Instance.AgregarPuntos(puntos);
            //Instancio las particulas de muerte
            GameObject pera = Instantiate(boom, transform.position, Quaternion.identity);
            pera.GetComponent<AudioSource>().clip = audiosEnemigos[2]; // Asigna el clip de audio
            pera.GetComponent<AudioSource>().Play();
            Destroy(pera, 1f); // Destruye el objeto de sonido después de 1 segundo
            Instantiate(particulasDead, transform.position, Quaternion.identity);
            muertoEnemy = true;
            MuertoBoss();
        }
        else if (!muertoEnemy)
        {
            audioSourceEnemigos.PlayOneShot(audiosEnemigos[1]);
            animator.SetTrigger("Hit");
            StartCoroutine(EnemigoSeEmpuja(empujeBala));
        }
    }
    public void RecibirDanyoNecro( float danoRecibido)
    {
        vidaEnemigo -= danoRecibido;
        if (vidaEnemigo <= 0 && !muertoEnemy)
        {
            FindAnyObjectByType<SpawnBoss>().BossMuertoNigro(this.gameObject);
            ScoreManager.Instance.AgregarPuntos(puntos);
            //Instancio las particulas de muerte
            GameObject pera = Instantiate(boom, transform.position, Quaternion.identity);
            pera.GetComponent<AudioSource>().clip = audiosEnemigos[2]; // Asigna el clip de audio
            pera.GetComponent<AudioSource>().Play();
            Destroy(pera, 1f); // Destruye el objeto de sonido después de 1 segundo
            Instantiate(particulasDead, transform.position, Quaternion.identity);
            muertoEnemy = true;
            MuertoBoss();
        }
        else if (!muertoEnemy)
        {
            audioSourceEnemigos.PlayOneShot(audiosEnemigos[1]);
            animator.SetTrigger("Hit");
            
        }
    }
    public void ScriptSpawnBoss(SpawnNecromancer spawnBoss2)
    {
        //Recibe el script del spawn
        spawnBoss = spawnBoss2;
    }
    public void MuertoBoss()
    {
        //Aumenta el contador de muertes
        animator.SetTrigger("Muerto");
        //EL enemigo deja de moverse
        speed = 0;
        //Desactiva el collider del enemigo
        GetComponent<Collider2D>().enabled = false;
        //Desactiva el rigidbody del enemigo
        GetComponent<Rigidbody2D>().isKinematic = true;
        GameManager gameManager = FindObjectOfType<GameManager>();
        SpawnEnemigos spawnEnemigos = FindObjectOfType<SpawnEnemigos>();
        gameManager.AumentarContadorMuertes();
        spawnEnemigos.EnemigoMuerto();
        GetComponent<SpawnNecromancer>().DestruirTodos();
        Destroy(gameObject, 1f);
    }
    public void Empujar(Vector2 empuje3)
    {
        StartCoroutine(EnemigoSeEmpuja(empuje3));
    }
      private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerScript>().PlayerDañado();
        }
    }
    IEnumerator EnemigoSeEmpuja(Vector2 empuje2)
    {
        knokeadoSTOP = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        float tiempo = 0;
        while (tiempo < tiempoKnockUP)
        {
            //Mueve en la direccion del empuje
            transform.position += (Vector3)empuje2 * Time.deltaTime * fuerzaDeEmpuje;
            tiempo += Time.deltaTime;
            yield return null;
        }
        knokeadoSTOP = true;

    }
}
