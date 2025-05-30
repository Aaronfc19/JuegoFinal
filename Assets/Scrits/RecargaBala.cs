using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecargaBala : MonoBehaviour
{
    [SerializeField] private float tiempoDeRecoger;
    [SerializeField] private GameObject sonidoRecarga;
    [SerializeField] private AudioClip recargaClip;
    private bool recogible;
    private bool soloUnaVEz = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && recogible && soloUnaVEz)
        {
            soloUnaVEz = false;
            Debug.Log("Recarga de balas");
            GameObject pera = Instantiate(sonidoRecarga, transform.position, Quaternion.identity);
            pera.GetComponent<AudioSource>().clip = recargaClip; // Asigna el clip de audio
            pera.GetComponent<AudioSource>().Play();
            Destroy(pera, 1f); // Destruye el objeto de sonido después de 1 segundo
            GameManager.gameManager.RecargarBalas();
            Destroy(this.gameObject);
        }
    }
    public void Update()
    {
        if (!recogible)
        {
        tiempoDeRecoger -= Time.deltaTime;
        if (tiempoDeRecoger <= 0)
        {
            recogible = true;
        }
        }
    }
}
