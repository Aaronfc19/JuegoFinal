using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera cameraScene;
    [SerializeField] private int cargadorArma;
    private float vidaActual;
    [SerializeField] private float dañoQueRecibo;
    [SerializeField] private bool playerMuerto;
    [SerializeField] private float vidaMaxima;
    [SerializeField] private Slider sliderPlayer;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        cameraScene = Camera.main;
        vidaActual = vidaMaxima;

    }


    // Update is called once per frame
    void Update()
    {
        Vector2 moverInput = playerInput.actions["Mover"].ReadValue<Vector2>();
        //Muevo al personaje en un entorno 2d con el input del jugador
        transform.position += new Vector3(moverInput.x, moverInput.y, 0) * speed * Time.deltaTime;
        //Hago que el personaje mire hacia la dirección en la que se mueve
        if (moverInput.x > 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moverInput.x < 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        //Lista esto no miran en la dirección en la que se mueven
        

        //Añado la variable del movimiento al animator
        animator.SetFloat("Speed", moverInput.magnitude);
        //Hago que la cámara siga al personaje
        cameraScene.transform.position = new Vector3(transform.position.x, transform.position.y, cameraScene.transform.position.z);
        
    }
    public void PlayerDañado()
    {
        //Recibe daño del jugador
        vidaActual -= dañoQueRecibo;
        GameManager.gameManager.CambiarVida(vidaActual);
        if (vidaActual > 0)
        {
            
            //pongo el color del sprite en rojo durante 0.5 segundos
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("ResetColor", 0.5f);
        }
        if (vidaActual <= 0 && !playerMuerto)
        {
            //desactivo el animator de sangre
            animator.SetTrigger("Dead");
            Muerto();
        }
    }
    private void ResetColor()
    {
        //reseteo el color del sprite
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void Muerto()
    {
        //desactivo a los hijos
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
        //Pongo la velocidad del jugador a 0
        speed = 0;
    }
    //Hago un get para el numero de balas
    public int GetCargadorArma()
    {
        return cargadorArma;
    }
    public float GetVidaPlayer()
    {
        return vidaMaxima;
    }
    public Slider GetBarraVida()
    {
        return sliderPlayer;
    }
}
