using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecargaBalaTodas : MonoBehaviour
{
   
    [SerializeField] private GameObject sonidoRecarga;
    [SerializeField] private AudioClip recargaClip;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Recarga de todas las balas");
            GameObject pera = Instantiate(sonidoRecarga, transform.position, Quaternion.identity);
            pera.GetComponent<AudioSource>().clip = recargaClip; // Asigna el clip de audio
            pera.GetComponent<AudioSource>().Play();
            Destroy(pera, 1f); // Destruye el objeto de sonido después de 1 segundo
            GameManager.gameManager.RecargarTodasLasBalas();
            Destroy(this.gameObject);
        }
    }
}
