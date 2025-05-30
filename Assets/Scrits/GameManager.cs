using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [Header("UI")]
    [SerializeField] private TMP_Text contadorMuertes;
    [SerializeField] private TMP_Text relojTiempo;
    [SerializeField] private List<GameObject> balasUI = new();
    [SerializeField] private Slider sliderVida;
    [SerializeField] private Slider sliderBoss;
    [SerializeField] private Animator animatorBlood;

    [Header("Estado")]
    [SerializeField] private int bajasEnemigas;
    [SerializeField] private float segundos;
    [SerializeField] private float minutos;
    private int numeroBalas;

    private GameObject playerAsignado;

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

    private void Start()
    {
        StartCoroutine(WaitForPlayerAndInitialize());
        ScoreManager.Instance.ReiniciarPuntos();
        ScoreManager.Instance.EmpezarCronometro();
    }

    private IEnumerator WaitForPlayerAndInitialize()
    {
        // Espera hasta que se haya instanciado el jugador
        while (GameObject.FindGameObjectWithTag("Player") == null)
        {
            yield return null;
        }

        playerAsignado = GameObject.FindGameObjectWithTag("Player");

        var playerScript = playerAsignado.GetComponent<PlayerScript>();
        if (playerScript == null)
        {
            Debug.LogError("PlayerScript no encontrado en el jugador instanciado.");
            yield break;
        }

        sliderVida = playerScript.GetBarraVida();
        sliderVida.maxValue = playerScript.GetVidaPlayer();
        sliderVida.value = sliderVida.maxValue;

        // SpawnBoss puede que no exista al inicio, esperamos un frame si es necesario
        yield return null;
        var spawnBoss = GameObject.FindGameObjectWithTag("SpawnBoss")?.GetComponent<SpawnBoss>();
        if (spawnBoss != null)
        {
            sliderBoss.maxValue = spawnBoss.GetTiempoEntreSpawn();
            sliderBoss.value = spawnBoss.GetTiempoDeSpawn();
        }

        numeroBalas = Mathf.Min(playerScript.GetCargadorArma(), balasUI.Count);
        ActivarBalas(numeroBalas);
    }

    private void Update()
    {
        ActualizarReloj();
    }

    private void ActualizarReloj()
    {
        segundos += Time.deltaTime;
        if (segundos >= 60)
        {
            minutos++;
            segundos = 0;
        }
        relojTiempo.text = minutos.ToString("00") + ":" + Mathf.FloorToInt(segundos).ToString("00");
    }

    public void CambiarTiempoBoss(float tiempoNecro) => sliderBoss.value = tiempoNecro;

    public void CambiarVida(float vida)
    {
        sliderVida.value = vida;
        animatorBlood.SetTrigger("DañoJugador");
    }

    public void CambiarVidaMaxima(float vidaMaxima) => sliderVida.maxValue = vidaMaxima;

    public void AumentarContadorMuertes()
    {
        bajasEnemigas++;
        contadorMuertes.text = bajasEnemigas.ToString();
    }

    public void ActivarBalas(int balasActivas)
    {
        for (int i = 0; i < balasActivas; i++)
        {
            balasUI[i].SetActive(true);
        }
    }

    public void RecorreListaBalasUI()
    {
        foreach (var bala in balasUI)
        {
            if (!bala.activeSelf)
            {
                bala.SetActive(true);
                break;
            }
        }
    }

    public bool ConsutoBala()
    {
        for (int i = 0; i < numeroBalas; i++)
        {
            var balaUI = balasUI[i].GetComponent<BalaUI>();
            if (balaUI.EstaActiva())
            {
                balaUI.Disparar();
                return true;
            }
        }
        return false;
    }

    public void RecargarBalas()
    {
        for (int i = numeroBalas - 1; i >= 0; i--)
        {
            var balaUI = balasUI[i].GetComponent<BalaUI>();
            if (!balaUI.EstaActiva())
            {
                balaUI.Recargar();
                return;
            }
        }
    }

    public void RecargarTodasLasBalas()
    {
        for (int i = 0; i < numeroBalas; i++)
        {
            balasUI[i].GetComponent<BalaUI>().Recargar();
        }
    }


    public float GetMinutos() => minutos;
    public float GetSegundos() => segundos;
    public int GetNumeroBalas() => numeroBalas;
}