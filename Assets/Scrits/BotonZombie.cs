using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonZombie : MonoBehaviour
{
//Quiero que al apretar el boton se dispare una animacio�n por trigger, haz una mecion al animator y al trigger
    public Animator animator; // Referencia al componente Animator del objeto
    // M�todo que se llama cuando se hace clic en el bot�n
    public void MatarAlZombie()
    {
        // Activa el trigger en el Animator
        animator.SetTrigger("OnClick");
    }
    public void CambiarEscena()
    {
        // Cambia a la escena especificada
        UnityEngine.SceneManagement.SceneManager.LoadScene("SeleccionPersonaje");
    }

}
