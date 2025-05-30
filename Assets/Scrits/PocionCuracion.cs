using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocionCuracion : MonoBehaviour
{
    [SerializeField] private float cantidadCuracion = 10f;
    [SerializeField] private float tiempoDeRecoger = 1f;
    [SerializeField] private GameObject sonidoDeRecoger;
    [SerializeField] private AudioClip recogerClip;
    private bool recogible = false;
    private bool soloUnaVez = true;

    private void Update()
    {
        if (!recogible)
        {
            tiempoDeRecoger -= Time.deltaTime;
            if (tiempoDeRecoger <= 0f)
                recogible = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogible && soloUnaVez && other.CompareTag("Player"))
        {
            soloUnaVez = false;
            GameObject pera = Instantiate(sonidoDeRecoger, transform.position, Quaternion.identity);
            pera.GetComponent<AudioSource>().clip = recogerClip; // Asigna el clip de audio
            pera.GetComponent<AudioSource>().Play();
            Destroy(pera, 1f); // Destruye el objeto de sonido después de 1 segundo
            PlayerScript player = other.GetComponent<PlayerScript>();
            player.Curar(cantidadCuracion);
            Destroy(gameObject);
        }
    }
}
