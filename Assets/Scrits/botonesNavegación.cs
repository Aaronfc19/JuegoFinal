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
    //al hacer click va al menu de creditos
    public void CargarEscenaCreditos()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Creditos");
    }
    //Al hacer click va al menu de seleccion de personajes
    public void CargarEscenaSeleccionPersonaje()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SeleccionPersonaje");
    }
    //al hacer clik va a las escenas de como jugar
    public void CargarEscenaComoJugar()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Comojugar");
    }
    //Al hacer click va a la escena de puntuaciones
    public void CargarEscenaPuntuaciones()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Puntuacíones");
    }
    //Cierra la aplicacion
    public void SalirAplicacion()
    {
        Application.Quit();
        Debug.Log("Saliendo de la aplicacion...");
    }
    //recarga la escena actual
    public void RecargarEscenaActual()
    {
        string escenaActual = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(escenaActual);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
