using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class ArmaPistola : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float tamanyoDelCirculo;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletNumber;
    [SerializeField] private float anguloDispersion;
    [SerializeField] private float tiempo;
    [SerializeField] private float tiempoEperando;
    [SerializeField] private bool EsperandoTiempo;
    [SerializeField] private bool disparandoPum;
    [SerializeField] private Transform targetPosition;
    private bool tengoEnemigo;
    //[SerializeField] private AudioSource escopeTazo;

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

        //Si el enemigo sale del rango de vision, lo elimino
        if (enemyPosition == null)
        {
            return;
        }
        if (Vector2.Distance(transform.position, enemyPosition.position) > tamanyoDelCirculo)
        {
            enemyPosition = null;
            return;
        }
        //El arma detecta al enemigo dentro del rango de vision
        if (Vector2.Distance(transform.position, enemyPosition.position) < tamanyoDelCirculo)
        {
            //Hago que el arma gire hacia el enemigo
            Vector3 direction = enemyPosition.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            if (enemyPosition.position.x > this.transform.position.x)
            {
                //transform.localScale = new Vector3(1, 1, 1);
                GetComponentInChildren<SpriteRenderer>().flipY = false;
            }
            else if (enemyPosition.position.x < this.transform.position.x)
            {
                //transform.localScale = new Vector3(-1, 1, 1);
                GetComponentInChildren<SpriteRenderer>().flipY = true;
            }
            //Disparo el arma
            disparandoPum = true;
            if (disparandoPum)
            {
                if (EsperandoTiempo)
                {
                    EnEspera();
                }
                else
                {
                    Debug.Log("PUM");
                    DisparoPistola();
                    //escopeTazo.Play();
                    EsperandoTiempo = true; tiempoEperando = tiempo;
                    disparandoPum = false;
                }
            }


        }
        else
        {
            //Si el enemigo no está dentro del rango de vision, el arma no gira
            transform.rotation = Quaternion.identity;
        }
      
    }
    private void OnTriggerStay2D(Collider2D posibleEnemigo)
    {
        if (enemyPosition == null && posibleEnemigo.gameObject.CompareTag("Enemy"))
        {
            //Busco al enemigo
            enemyPosition = posibleEnemigo.gameObject.transform;  
        }
    }
   
    private void DisparoPistola() 
    {
        if (GameManager.gameManager.ConsutoBala())
        {
             // Disparar múltiples perdigones
         
        // Instanciamos la bala
        GameObject spawnedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // Generamos un ángulo de dispersión aleatorio en un cono
        //Calculamos la direccion  entre el bulletSpawnPoint y el enemigo
        Vector2 direction = enemyPosition.position - bulletSpawnPoint.position;
        // Normalizamos la dirección
        direction.Normalize();
        spawnedBullet.GetComponent<ScriptBala>().SettDirection(direction);
        Destroy(spawnedBullet, 7);

        }
        else
        {
            Debug.Log("No hay balas");
        }

    }
    private void EnEspera()
    {
        if (GameManager.gameManager.GetNumeroBalas() <= 0)
        {
            return;
        }
        tiempoEperando -= Time.deltaTime;
        if (tiempoEperando <= 0)
        {
            EsperandoTiempo = false;
        }

    }
    //Creo un circulo para el rango de vision del arma
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tamanyoDelCirculo);
        
        

    }
    public Transform Direccion()
    {
        return enemyPosition;

    }
}
