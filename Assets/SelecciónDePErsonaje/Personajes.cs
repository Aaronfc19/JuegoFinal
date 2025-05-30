using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Personajes", menuName = "Personaje ", order = 1)]
public class Personajes : ScriptableObject
{
    public GameObject personajePrefab;
    public Sprite imagen;
    public string nombrePersonaje;
}
