using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaboom : MonoBehaviour

{
    [SerializeField] private float tiempoDeRecoger = 1f;
    [SerializeField] private ParticleSystem boomFinal;
    GameObject[] Enemigos;
    [SerializeField] private float daño = 999f; // ?? nuevo: daño a los enemigos
    [SerializeField] private GameObject audioSorceBOOM;
    [SerializeField] private AudioClip clipBOOM;
    private bool recogible = false;
    private bool soloUnaVez = true;

    private void Update()
    {
        if (!recogible)
        {
            tiempoDeRecoger -= Time.deltaTime;
            if (tiempoDeRecoger <= 0f)
            { 
                recogible = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogible && soloUnaVez && other.CompareTag("Player"))
        {
            soloUnaVez = false;
            ParticleSystem explosion = Instantiate(boomFinal, transform.position, Quaternion.identity);
            GameObject pera = Instantiate(audioSorceBOOM, transform.position, Quaternion.identity);
            pera.GetComponent<AudioSource>().clip = clipBOOM; // Asigna el clip de audio
            pera.GetComponent<AudioSource>().Play();
            Destroy(pera, 1f); // Destruye el objeto de sonido después de 1 segundo
            //Daño a los enemigos
            Enemigos = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemigo in Enemigos)
            {
                if (enemigo != null)
                {
                    if (enemigo.TryGetComponent(out EnemyBasicScript enemy))
                    {
                        enemy.RecibirDanyo(daño); // ?? ahora se pasa el daño
                    }
                    else if (enemigo.TryGetComponent(out NecromancerBoss necro))
                    {
                        necro.RecibirDanyoNecro(daño);
                    }
                    else if (enemigo.TryGetComponent(out EnemigoGeneradoPorNigro nigroMinion))
                    {
                        nigroMinion.RecibirDanyo(daño);
                    }
                }
            }
            Destroy(this.gameObject);
        }
    }
}
