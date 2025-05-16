using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecargaBala : MonoBehaviour
{
    [SerializeField] private float tiempoDeRecoger;
    private bool recogible;
    private bool soloUnaVEz = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && recogible && soloUnaVEz)
        {
            soloUnaVEz = false;
            Debug.Log("Recarga de balas");
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
