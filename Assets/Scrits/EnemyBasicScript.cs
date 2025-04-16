using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private bool touchPlayer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        touchPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Muevo al enemigo hacia el jugador
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        //Hago que el enemigo mire hacia la dirección en la que se mueve
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (transform.position.x > player.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //Si el enemigo toca el collider del jugador, se activa la variable touchPlayer 
        if (Vector2.Distance(transform.position, player.position) < 1f)
        {
            touchPlayer = true;
        }
        else
        {
            touchPlayer = false;
        }


    }
}
