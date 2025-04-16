using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaCuerpoaCuerpo : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotationSpeedATQ;
    [SerializeField] private float tamañoDelCirculo;
    [SerializeField] private float tamañoDelCirculoAtaque;
    //[SerializeField] private float timeToDestroy;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private float tiempoENtreAtaque;
    [SerializeField] private SpriteRenderer armaColor;
    [SerializeField] private float tiempo;
    [SerializeField] private float tiempoEperando;
    [SerializeField] private bool EsperandoTiempo;
    [SerializeField] private bool disparandoPum;
    // Start is called before the first frame update
    void Start()
    {

        EsperandoTiempo = false;
        tiempoEperando = tiempo;
        disparandoPum = false;

    }

    // Update is called once per frame
    void Update()
    {
        enemyPosition = GameObject.FindGameObjectWithTag("Enemy").transform;
        //El arma detecta al enemigo dentro del rango de vision
        if (Vector2.Distance(transform.position, enemyPosition.position) < tamañoDelCirculo)
        {
            //Hago que el arma gire hacia el enemigo
            Vector3 direction = enemyPosition.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            //Si el enemigo se encuentra dentro del rango de ataque se activa el ataque
            if (Vector2.Distance(transform.position, enemyPosition.position) < tamañoDelCirculoAtaque)
            {
                //Disparo el arma
                disparandoPum = true;
                if (disparandoPum)
                {
                    if (EsperandoTiempo)
                    {
                        EnEspera();
                        armaColor.material.color = Color.white;
                    }
                    else
                    {
                        Debug.Log("PUM");
                        Ataque360();
                        //escopeTazo.Play();
                        EsperandoTiempo = true; tiempoEperando = tiempo;
                        disparandoPum = false;
                    }
                }

            }
        }
        else
        {
            //El arma se queda mirando hacia la ultima posicion 
            Vector3 direction = enemyPosition.position - transform.position;
        }
    }
    private void Ataque360() 
    {
        //Si se activa esta variable, el arma hace un giro de 360 grados
        transform.Rotate(Vector3.forward * rotationSpeedATQ * Time.deltaTime);
        // el Sprite renderer cambia de color a blanco durante el ataque
        armaColor.material.color = Color.red;
    }
    private void EnEspera()
    {
       
            tiempoEperando -= Time.deltaTime;
            if (tiempoEperando <= 0)
            {
                EsperandoTiempo = false;
            }


    }
    //Creo un circulo para el rango de vision del arma
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tamañoDelCirculo);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, tamañoDelCirculoAtaque);
    }

}
