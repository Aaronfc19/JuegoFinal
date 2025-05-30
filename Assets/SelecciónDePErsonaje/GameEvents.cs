using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<GameObject> OnPlayerSpawned;

    public static void PlayerSpawned(GameObject player)
    {
        OnPlayerSpawned?.Invoke(player);
    }
}
