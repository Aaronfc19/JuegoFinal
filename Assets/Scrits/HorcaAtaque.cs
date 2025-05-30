using UnityEngine;

public class HorcaAtaque : MonoBehaviour
{
    [SerializeField] private float rangoDeteccion = 5f;
    [SerializeField] private float velocidadRotacion = 100f;
    [SerializeField] private Animator animator;
    [SerializeField] private float dano = 2f;
    [SerializeField] private LayerMask capaEnemigos;
    [SerializeField] private AudioSource horcaAtaque;
    private Transform enemigoActual;

    void Update()
    {
        BuscarEnemigo();

        if (enemigoActual != null)
        {
            Vector2 direccion = enemigoActual.position - transform.position;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            Quaternion rotacionObjetivo = Quaternion.Euler(0, 0, angulo);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.flipY = (enemigoActual.position.x < transform.position.x);
        }
       
    }

    private void BuscarEnemigo()
    {
        Collider2D[] detectados = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion, capaEnemigos);
        float distanciaMasCercana = Mathf.Infinity;
        Transform objetivo = null;

        foreach (Collider2D enemigo in detectados)
        {
            float distancia = Vector2.Distance(transform.position, enemigo.transform.position);
            if (distancia < distanciaMasCercana)
            {
                distanciaMasCercana = distancia;
                objetivo = enemigo.transform;
            }
        }

        enemigoActual = objetivo;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemigoActual == null || !collision.gameObject.CompareTag("Enemy")) return;
        horcaAtaque.Play();
        animator.SetTrigger("Ataca");
        Vector2 direccion = (collision.transform.position - transform.position).normalized;

        if (collision.gameObject.TryGetComponent(out EnemyBasicScript basic))
            basic.RecibirDanyo(direccion, dano);

        else if (collision.gameObject.TryGetComponent(out NecromancerBoss boss))
            boss.RecibirDanyoNecro(direccion, dano);

        else if (collision.gameObject.TryGetComponent(out EnemigoGeneradoPorNigro invocado))
            invocado.RecibirDanyo(direccion, dano);
    }

    public void AumentarDaño(float extra)
    {
        dano += extra;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
