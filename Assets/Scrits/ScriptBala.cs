using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBala : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SettDirection(Vector2 direction)
    {
        //Inicializo la dirección de la bala
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        //Muevo la bala en la dirección en la que se dispara
        if (direction == null||direction == Vector2.zero)
        {
            return;
        }
        transform.position += (Vector3)direction * bulletSpeed * Time.fixedDeltaTime;
    }

}
