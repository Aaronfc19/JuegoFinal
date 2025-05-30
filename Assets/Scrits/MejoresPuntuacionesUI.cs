using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MejoresPuntuacionesUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> textosPuntuaciones;
    private const string clavePuntuaciones = "MejoresPuntuaciones";

    private void Start()
    {
        List<ScoreEntry> lista = ObtenerPuntuacionesDesdePrefs();

        for (int i = 0; i < textosPuntuaciones.Count; i++)
        {
            if (i < lista.Count)
            {
                textosPuntuaciones[i].text = $"{i + 1}. {lista[i].nombreJugador} - {lista[i].puntuacion}";
            }
            else
            {
                textosPuntuaciones[i].text = $"{i + 1}. ---";
            }
        }
    }

    private List<ScoreEntry> ObtenerPuntuacionesDesdePrefs()
    {
        string json = PlayerPrefs.GetString(clavePuntuaciones, "");
        if (string.IsNullOrEmpty(json)) return new List<ScoreEntry>();
        return JsonUtility.FromJson<ScoreListWrapper>(json).lista;
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public string nombreJugador;
        public int puntuacion;
    }

    [System.Serializable]
    private class ScoreListWrapper
    {
        public List<ScoreEntry> lista;
    }
}