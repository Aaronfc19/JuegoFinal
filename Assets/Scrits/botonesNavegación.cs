using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class botonesNavegación : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //Al hacer click en el boton de la escena principal, se carga la escena de inicio
    public void CargarEscenaInicio()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Inicio");
    }
    //al hacer click va al menu de opciones
    public void CargarEscenaOpciones()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Opciones");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
