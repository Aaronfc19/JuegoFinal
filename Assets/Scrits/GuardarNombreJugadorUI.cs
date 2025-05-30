using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuardarNombreJugadorUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject panelPuntuacion;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text textoPuntuacionFinal;
    [SerializeField] private TMP_InputField inputNombre;
    [SerializeField] private Button botonGuardar;
    [SerializeField] private Button botonVerPuntuaciones;

    [Header("UI a Desactivar al Morir")]
    [SerializeField] private List<GameObject> elementosUIADesactivar;

    [Header("Animación")]
    [SerializeField] private float duracionFade = 1f;

    private void Start()
    {
        panelPuntuacion.SetActive(false);
    }

    public void MostrarPanelFinal()
    {
        foreach (var ui in elementosUIADesactivar)
        {
            if (ui != null)
                ui.SetActive(false);
        }
        ScoreManager.Instance.PausarCronometro(); // Pausar el cronómetro al finalizar el juego
        int puntuacionFinal = ScoreManager.Instance.CalcularPuntuacionFinal();
        textoPuntuacionFinal.text = $"Puntuación: {puntuacionFinal}";

        List<ScoreEntry> lista = ScoreManager.Instance.ObtenerLista();
        bool entraEnTop = lista.Count < 5 || puntuacionFinal > lista[^1].puntuacion;

        // Mostrar o esconder campos según si entra al top
        inputNombre.gameObject.SetActive(entraEnTop);
        botonGuardar.gameObject.SetActive(entraEnTop);
        botonVerPuntuaciones.gameObject.SetActive(!entraEnTop);

        panelPuntuacion.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void GuardarYMostrarRanking()
    {
        string nombre = inputNombre.text;
        if (string.IsNullOrWhiteSpace(nombre))
        {
            nombre = "Desconocido";
        }

        ScoreManager.Instance.GuardarPuntuacion(nombre);
        StartCoroutine(FadeOutYCambiarEscena("Puntuacíones"));
    }

    public void VerRankingSinGuardar()
    {
        StartCoroutine(FadeOutYCambiarEscena("Puntuacíones"));
    }

    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(tiempo / duracionFade);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutYCambiarEscena(string escenaDestino)
    {
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (tiempo / duracionFade));
            yield return null;
        }
        SceneManager.LoadScene(escenaDestino);
    }
}