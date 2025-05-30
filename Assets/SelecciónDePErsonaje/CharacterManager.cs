using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
  public static CharacterManager Instance;
    public List<Personajes> personajes;
    public void Awake()
    {
        if (CharacterManager.Instance == null)
        {
            CharacterManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
