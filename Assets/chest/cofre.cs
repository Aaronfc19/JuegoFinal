using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cofre : MonoBehaviour
{
    [SerializeField] private List<GameObject> objetosCofre = new List<GameObject>();
    [SerializeField] private Animator animator;
    private bool cofreAbierto = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    //Hago un ontrigger enter para que cuando el player entre al trigger se le de un objeto random del cofre
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cofreAbierto)
        {
            cofreAbierto = false;
            animator.SetTrigger("Abierto");
            //Busco un objeto random del cofre
            int random = Random.Range(0, objetosCofre.Count);
            //Instancio el objeto random en la posicion del cofre
            Instantiate(objetosCofre[random], transform.position, Quaternion.identity);
            //Destruyo el cofre
            Destroy(gameObject,1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
