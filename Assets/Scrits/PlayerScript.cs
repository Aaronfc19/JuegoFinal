using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera cameraScene;
    //Hago una lista para los game objects que no quiero que miren en la dirección en la que se mueven
    [SerializeField] private List<GameObject> gameObjectsToIgnore;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        cameraScene = Camera.main;
        //Lista esto no miran en la dirección en la que se mueven
        gameObjectsToIgnore = new List<GameObject>();

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

}
