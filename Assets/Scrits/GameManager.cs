using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    [SerializeField] private TMP_Text contadorMuertes;
    [SerializeField] private int bajasEnemigas;
    [SerializeField] private float segundos;
    [SerializeField] private float minutos;
    [SerializeField] private TMP_Text relojTiempo;
    [SerializeField] List<GameObject> balasUI = new() ;
    private int numeroBalas;
    [SerializeField] private Slider sliderVida;
    [SerializeField] private Slider sliderBoss;
    [SerializeField] private Animator animatorBlood;
    private GameObject playerAsignado;
    // Start is called before the first frame update
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //asigno el player
        if(playerAsignado == null)
        {
            playerAsignado = GameObject.FindGameObjectWithTag("Player");
        }
        animatorBlood.GetComponent<Animator>();
        sliderVida =playerAsignado.GetComponent<PlayerScript>().GetBarraVida();
        sliderBoss.maxValue = GameObject.FindGameObjectWithTag("SpawnBoss").GetComponent<SpawnBoss>().GetTiempoEntreSpawn();
        sliderBoss.value = GameObject.FindGameObjectWithTag("SpawnBoss").GetComponent<SpawnBoss>().GetTiempoDeSpawn();
        sliderVida.maxValue = playerAsignado.GetComponent<PlayerScript>().GetVidaPlayer();
        sliderVida.value = sliderVida.maxValue;
        numeroBalas = playerAsignado.GetComponent<PlayerScript>().GetCargadorArma();
        if (numeroBalas > balasUI.Count)
        {
            numeroBalas = balasUI.Count;
        }
        ActivarBalas(numeroBalas);  
    }

    // Update is called once per frame
    void Update()
    {
        TimepoCronometro();
    }
    public void CambiarTiempoBoss(float tiempoNecro)
    {
        sliderBoss.value = tiempoNecro;
    }
    public void CambiarVida(float vida)
    {
        sliderVida.value = vida;
        //si el jugador recibe daño, se activa el animator de sangre
        animatorBlood.SetTrigger("DañoJugador");
    }
    public void CambiarVidaMaxima(float vidaMaxima)
    {
        sliderVida.maxValue = vidaMaxima;
    }
    //Hago un reloj que cuenta el tiempo
    public void TimepoCronometro()
    {
        //Contador de tiempo
        segundos += Time.deltaTime;
        if (segundos >= 60)
        {
            minutos++;
            segundos = 0;
        }
        relojTiempo.text = minutos.ToString("00") + ":" + Mathf.FloorToInt(segundos).ToString("00");
    }
    public void AumentarContadorMuertes()
    {
        bajasEnemigas++;
        //punto.Play();
        contadorMuertes.text = bajasEnemigas.ToString();
    }
    public void RecorreListaBalasUI()
    {
        for (int i = 0; i < balasUI.Count; i++)
        {
            if (balasUI[i].activeSelf == false)
            {
                balasUI[i].SetActive(true);
                break;
            }
        }

    }
    public void ActivarBalas(int balasActivas)
    {
        for (int i = 0; i < balasActivas; i++)
        {
            balasUI[i].SetActive(true);
        }
    }
    public bool ConsutoBala()
    {
        for (int i = 0; i < numeroBalas; i++)
        {
            if (balasUI[i].GetComponent<BalaUI>().EstaActiva())
            {
                balasUI[i].GetComponent<BalaUI>().Disparar();
                return true;
            }
        }
        return false;
    }
    public void RecargarBalas()
    {
        
        for (int i = numeroBalas -1; i >= 0; i--)
        {
            if (!balasUI[i].GetComponent<BalaUI>().EstaActiva())
            {
                balasUI[i].GetComponent<BalaUI>().Recargar();
                //Salgo del bucle
                return;
            }
            
        }
    }
    public void RecargarTodasLasBalas()
    {
        //Recargo todas las balas de numeroBalas
        for (int i = 0; i < numeroBalas; i++)
        {
            balasUI[i].GetComponent<BalaUI>().Recargar();
        }
    }
    //Getter minutos y segundos
    public float GetMinutos()
    {
        return minutos;
    }
    public float GetSegundos()
    {
        return segundos;
    }
    public int GetNumeroBalas()
    {
        return numeroBalas;
    }
}
