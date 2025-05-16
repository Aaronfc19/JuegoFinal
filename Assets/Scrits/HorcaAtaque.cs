using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorcaAtaque : MonoBehaviour
{
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private float tamanyoDelCirculo;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Si el enemigo sale del rango de vision, lo elimino
        if (enemyPosition == null)
        {
            return;
        }
        if (Vector2.Distance(transform.position, enemyPosition.position) > tamanyoDelCirculo)
        {
            enemyPosition = null;
            return;
        }
        //El arma detecta al enemigo dentro del rango de vision
        if (Vector2.Distance(transform.position, enemyPosition.position) < tamanyoDelCirculo)
        {
            //Hago que el arma gire hacia el enemigo
            Vector3 direction = enemyPosition.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            if (enemyPosition.position.x > this.transform.position.x)
            {
                //transform.localScale = new Vector3(1, 1, 1);
                GetComponentInChildren<SpriteRenderer>().flipY = false;
            }
            else if (enemyPosition.position.x < this.transform.position.x)
            {
                //transform.localScale = new Vector3(-1, 1, 1);
                GetComponentInChildren<SpriteRenderer>().flipY = true;
            }
        }
        else
        {
            //Si el enemigo no está dentro del rango de vision, el arma no gira
            transform.rotation = Quaternion.identity;
        }

    }

    private void OnTriggerStay2D(Collider2D posibleEnemigo)
    {
        if (enemyPosition == null && posibleEnemigo.gameObject.CompareTag("Enemy"))
        {
            //Busco al enemigo
            enemyPosition = posibleEnemigo.gameObject.transform;
        }
    }
   private void OnCollisionEnter2D (Collision2D collision)
    {
        //Si el enemigo sale del rango de vision, lo elimino
        if (enemyPosition != null && collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Ataca");
            //muevo al enemigo hacia la direccion contraria a la pala
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            if (collision.gameObject.GetComponent<EnemyBasicScript>())
            {
                collision.gameObject.GetComponent<EnemyBasicScript>().RecibirDanyo(direction);
            }
            if (collision.gameObject.GetComponent<NecromancerBoss>())
            {
                collision.gameObject.GetComponent<NecromancerBoss>().RecibirDanyoNecro(direction);
            }
            if (collision.gameObject.GetComponent<EnemigoGeneradoPorNigro>())
            {
                collision.gameObject.GetComponent<EnemigoGeneradoPorNigro>().RecibirDanyo(direction);
            }
        }
    }
    //Dibujo los circulos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tamanyoDelCirculo);
    }
}
