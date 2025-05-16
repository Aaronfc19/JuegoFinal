using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecargaBalaTodas : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.gameManager.RecargarTodasLasBalas();
            Destroy(this.gameObject);
        }
    }
}
