using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuSeleccionPersonaje : MonoBehaviour
{
    private int index;
    [SerializeField] private Image personajes;
    [SerializeField] private TextMeshProUGUI nombrePersonaje;
    private CharacterManager characterManager;
    private void Start()
    {
        characterManager = CharacterManager.Instance;
        index = PlayerPrefs.GetInt("IndexPersonaje");
        if (index >= characterManager.personajes.Count)
        {
            index = 0;
        }
        CambiarPAntalla();
    }
    private void CambiarPAntalla()
    {
        PlayerPrefs.SetInt("IndexPersonaje", index);
        personajes.sprite = characterManager.personajes[index].imagen;
        nombrePersonaje.text = characterManager.personajes[index].nombrePersonaje;
    }
    public void SiguientePersonaje()
    {
        if (index == characterManager.personajes.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        CambiarPAntalla();
    }
    public void AnteriorPersonaje()
    {
        if (index == 0)
        {
            index = characterManager.personajes.Count - 1;
        }
        else
        {
            index--;
        }
        CambiarPAntalla();
    }
    public void JugarJuego()
    {
        PlayerPrefs.SetInt("IndexPersonaje", index); // <--- Asegúrate de guardar el personaje elegido
        AudioManager.instance.PlayAudioClip(1);
        SceneManager.LoadScene("Juego");

    }
}
