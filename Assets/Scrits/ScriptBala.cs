using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBala : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float fuerzaImpulso;
    [SerializeField] private float daño; // ?? nuevo: daño de la bala

    public void SettDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void SetDaño(float nuevoDaño) // ?? nuevo: setter público para el daño
    {
        daño = nuevoDaño;
    }

    private void FixedUpdate()
    {
        if (direction == Vector2.zero) return;
        transform.position += (Vector3)direction * bulletSpeed * Time.fixedDeltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector2 empuje = collision.transform.position - transform.position;

            if (collision.TryGetComponent(out EnemyBasicScript enemy))
            {
                enemy.RecibirDanyo(empuje, daño); // ?? ahora se pasa el daño
            }
            else if (collision.TryGetComponent(out NecromancerBoss necro))
            {
                necro.RecibirDanyoNecro(empuje, daño);
            }
            else if (collision.TryGetComponent(out EnemigoGeneradoPorNigro nigroMinion))
            {
                nigroMinion.RecibirDanyo(empuje, daño);
            }

            Destroy(gameObject);
        }
    }
    public void AumentarDaño(float extra)
    {
        daño += extra; // ?? nuevo: método para aumentar el daño de la bala
    }
}

