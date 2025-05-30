using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicioJugador : MonoBehaviour
{
    void Start()
    {
        int indexJugador = PlayerPrefs.GetInt("IndexPersonaje", 0);
        GameObject jugador = Instantiate(CharacterManager.Instance.personajes[indexJugador].personajePrefab, transform.position, Quaternion.identity);
        GameEvents.PlayerSpawned(jugador); // Avisamos que el jugador fue creado
    }
}