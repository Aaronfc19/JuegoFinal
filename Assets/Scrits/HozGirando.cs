using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HozGirando : MonoBehaviour
{
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private float dano = 1f; // Da�o que inflige la hoz
    [SerializeField] private AudioSource sonidoHoz; // Sonido de la hoz

    void Update()
    {
        // No es necesario por ahora, pero se deja si m�s adelante se requiere l�gica en Update
    }

    private void OnTriggerStay2D(Collider2D posibleEnemigo)
    {
        if (enemyPosition == null && posibleEnemigo.CompareTag("Enemy"))
        {
            enemyPosition = posibleEnemigo.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyPosition != null && collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;

            // Se chequea cada tipo de enemigo y se le aplica da�o
            var enemyBasic = collision.gameObject.GetComponent<EnemyBasicScript>();
            if (enemyBasic != null)
            {
                sonidoHoz.Play(); // Reproduce el sonido de la hoz al atacar
                enemyBasic.RecibirDanyo(direction, dano);
                return;
            }

            var boss = collision.gameObject.GetComponent<NecromancerBoss>();
            if (boss != null)
            {
                sonidoHoz.Play(); // Reproduce el sonido de la hoz al atacar
                boss.RecibirDanyoNecro(direction, dano);
                return;
            }

            var invocado = collision.gameObject.GetComponent<EnemigoGeneradoPorNigro>();
            if (invocado != null)
            {
                sonidoHoz.Play(); // Reproduce el sonido de la hoz al atacar
                invocado.RecibirDanyo(direction, dano);
                return;
            }
        }
    }
    public void AumentarDa�o(float extra)
    {
        dano += extra; // ?? nuevo: m�todo para aumentar el da�o de la bala
    }
}