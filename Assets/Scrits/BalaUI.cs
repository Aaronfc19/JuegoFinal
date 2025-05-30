using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalaUI : MonoBehaviour
{
    [SerializeField] private RawImage balaUI;
    private bool isActive = true;
    // Start is called before the first frame update

    public bool EstaActiva()
    {
        return isActive;

    }
    public void Disparar()
    {
        balaUI.color = new Color(1, 1, 1, 0);
        isActive = false;
    }
    public void Recargar()
    {
        balaUI.color = new Color(1, 1, 1, 1);
        isActive = true;
    }
}
