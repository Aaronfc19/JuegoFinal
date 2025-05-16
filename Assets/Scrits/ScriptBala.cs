using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBala : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float fuerzaImpulso;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SettDirection(Vector2 direction)
    {
        //Inicializo la dirección de la bala
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        //Muevo la bala en la dirección en la que se dispara
        if (direction == null||direction == Vector2.zero)
        {
            return;
        }
        transform.position += (Vector3)direction * bulletSpeed * Time.fixedDeltaTime;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Si la bala toca al enemigo, se destruye
        if (collision.CompareTag("Enemy"))
        {
            //muevo al enemigo hacia la direccion contraria a la bala
           Vector2 direction = collision.transform.position - transform.position;
            //collision.GetComponent<Rigidbody2D>().AddForce(direction.normalized * fuerzaImpulso, ForceMode2D.Impulse);
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

            Destroy(gameObject);
        }
    }
}
