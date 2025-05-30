using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalaGiroAtaque : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float tiempoAtaque;
    [SerializeField] private float circuloEmpuje;
    [SerializeField] private float dano = 1f; // Daño que inflige la pala
    [SerializeField] private AudioSource sonidoPala; // Sonido de la pala al atacar
    private float tiempoDeCooldown;

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
                Vector2 direction = (collision.transform.position - transform.position).normalized;

                if (collision.TryGetComponent(out EnemyBasicScript basic))
                {
                    basic.RecibirDanyo(direction, dano);
                    continue;
                }

                if (collision.TryGetComponent(out NecromancerBoss boss))
                {
                    boss.RecibirDanyoNecro(direction, dano);
                    continue;
                }

                if (collision.TryGetComponent(out EnemigoGeneradoPorNigro invocado))
                {
                    invocado.RecibirDanyo(direction, dano);
                    continue;
                }
            }
        }
    }

    void Update()
    {
        tiempoDeCooldown -= Time.deltaTime;
        if (tiempoDeCooldown <= 0)
        {
            tiempoDeCooldown = tiempoAtaque;
            sonidoPala.Play(); // Reproduce el sonido de la pala al atacar
            animator.SetTrigger("Ataca");
        }
    }

    // Este método lo puede llamar un Animation Event desde la animación de ataque
    public void EjecutarGolpe()
    {
        EmpujarPala();
    }
    public void AumentarDaño(float extra)
    {
        dano += extra; // ?? nuevo: método para aumentar el daño de la bala
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circuloEmpuje);
    }
}