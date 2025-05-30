using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBala : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float fuerzaImpulso;
    [SerializeField] private float da�o; // ?? nuevo: da�o de la bala

    public void SettDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void SetDa�o(float nuevoDa�o) // ?? nuevo: setter p�blico para el da�o
    {
        da�o = nuevoDa�o;
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
                enemy.RecibirDanyo(empuje, da�o); // ?? ahora se pasa el da�o
            }
            else if (collision.TryGetComponent(out NecromancerBoss necro))
            {
                necro.RecibirDanyoNecro(empuje, da�o);
            }
            else if (collision.TryGetComponent(out EnemigoGeneradoPorNigro nigroMinion))
            {
                nigroMinion.RecibirDanyo(empuje, da�o);
            }

            Destroy(gameObject);
        }
    }
    public void AumentarDa�o(float extra)
    {
        da�o += extra; // ?? nuevo: m�todo para aumentar el da�o de la bala
    }
}

