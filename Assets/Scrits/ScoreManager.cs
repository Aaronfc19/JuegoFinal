using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreEntry
{
    public string nombreJugador;
    public int puntuacion;

    public ScoreEntry(string nombreJugador, int puntuacion)
    {
        this.nombreJugador = nombreJugador;
        this.puntuacion = puntuacion;
    }
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int puntosActuales;
    private float tiempoSobrevivido;
    private bool contandoTiempo = false;

    private const int maxPuntuaciones = 5;
    private const string clavePuntuaciones = "MejoresPuntuaciones";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (contandoTiempo)
        {
            tiempoSobrevivido += Time.deltaTime;
        }
    }

    public void EmpezarCronometro()
    {
        tiempoSobrevivido = 0f;
        contandoTiempo = true;
    }

    public void PausarCronometro()
    {
        contandoTiempo = false;
    }

    public void ReanudarCronometro()
    {
        contandoTiempo = true;
    }

    public void DetenerCronometro()
    {
        contandoTiempo = false;
    }

    public void AgregarPuntos(int cantidad)
    {
        puntosActuales += cantidad;
    }

    public int CalcularPuntuacionFinal()
    {
        return puntosActuales + Mathf.FloorToInt(tiempoSobrevivido);
    }

    public void GuardarPuntuacion(string nombreJugador)
    {
        int puntuacionFinal = CalcularPuntuacionFinal();
        List<ScoreEntry> lista = ObtenerLista();

        lista.Add(new ScoreEntry(nombreJugador, puntuacionFinal));
        lista.Sort((a, b) => b.puntuacion.CompareTo(a.puntuacion));
        if (lista.Count > maxPuntuaciones) lista.RemoveAt(lista.Count - 1);

        string json = JsonUtility.ToJson(new ScoreListWrapper(lista));
        PlayerPrefs.SetString(clavePuntuaciones, json);
        PlayerPrefs.Save();
    }

    public List<ScoreEntry> ObtenerLista()
    {
        string json = PlayerPrefs.GetString(clavePuntuaciones, "");
        return string.IsNullOrEmpty(json) ? new List<ScoreEntry>() : JsonUtility.FromJson<ScoreListWrapper>(json).lista;
    }

    public void ReiniciarPuntos()
    {
        puntosActuales = 0;
        tiempoSobrevivido = 0f;
        contandoTiempo = false;
    }

    [System.Serializable]
    private class ScoreListWrapper
    {
        public List<ScoreEntry> lista;
        public ScoreListWrapper(List<ScoreEntry> lista) => this.lista = lista;
    }
}