using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalaGiroAtaque : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float tiempoAtaque;
    [SerializeField] private float circuloEmpuje;
    private float tiempoDeCooldown;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        tiempoDeCooldown = tiempoAtaque;
    }
    private void EmpujarPala()
    {
        Collider2D[] collisionMelee = Physics2D.OverlapCircleAll(transform.position, circuloEmpuje);

        foreach (Collider2D collision in collisionMelee)
        {
            if (collision.CompareTag("Enemy"))
            {
                //Debug.Log("PUM");
                //muevo al enemigo hacia la direccion contraria a la pala
                Vector2 direction = (collision.transform.position - transform.position).normalized;
                if (collision.gameObject.GetComponent<EnemyBasicScript>())
                {
                    collision.GetComponent<EnemyBasicScript>().RecibirDanyo(direction);
                }
                if (collision.gameObject.GetComponent<NecromancerBoss>())
                {
                    collision.GetComponent<NecromancerBoss>().RecibirDanyoNecro(direction);
                }
                if (collision.gameObject.GetComponent<EnemigoGeneradoPorNigro>())
                {
                    collision.GetComponent<EnemigoGeneradoPorNigro>().RecibirDanyo(direction);
                }
            }
        }
     
        
    }

    // Update is called once per frame
    void Update()
    {
        //Pongo un temporizador que dispare la animacion de ataque automaticamnte cada x segundos
      
            tiempoDeCooldown -= Time.deltaTime;
            if (tiempoDeCooldown <= 0)
            {
                tiempoDeCooldown = tiempoAtaque;
                animator.SetTrigger("Ataca");
            }
        
        
    }
    private void OnDrawGizmosSelected()
    {
        //Dibujo el circulo de empuje
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circuloEmpuje);
    }
}
