using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoGeneradoPorNigro : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private float vidaEnemigo;
    [SerializeField] private float danoRecibido;
    [SerializeField] private float tiempoKnockUP;
    [SerializeField] private float fuerzaDeEmpuje;
    private SpawnNecromancer spawnNecro;
    private bool knokeadoSTOP;
    private bool muertoEnemy;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
    }
    public void ScriptNecromancer(SpawnNecromancer spawnNecro2)
    {
        //Recibe el script del spawn
        spawnNecro = spawnNecro2;
    }
    public void RecibirDanyo(Vector2 empujeBala)
    {
        //Recibe daño del jugador
        vidaEnemigo -= danoRecibido;
        if (vidaEnemigo > 0)
        {
            animator.SetTrigger("Hit");
            StartCoroutine(EnemigoSeEmpuja(empujeBala));
        }
        if (vidaEnemigo <= 0 && !muertoEnemy)
        {
            muertoEnemy = true;
            Muerto();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerScript>().PlayerDañado();
        }
    }
    public void Muerto()
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
        //Accedo al Script del necromancer y uso la variable EliminoEnemigosNigro();
        spawnNecro.EnemigoMuertoNigro(this.gameObject);
        gameManager.AumentarContadorMuertes();
        
    }
    public void Empujar(Vector2 empuje3)
    {
        StartCoroutine(EnemigoSeEmpuja(empuje3));
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
