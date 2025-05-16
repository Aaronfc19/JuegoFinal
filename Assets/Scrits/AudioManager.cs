using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of AudioManager exists
    public static AudioManager instance;
    [SerializeField] private float volumen;
    [SerializeField] private float velocidad;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }
    public void Start()
    {
       SetAudio(volumen);
        SetVelocidad(velocidad);
    }
    public void SetAudio(float audio)
    {
        AudioListener.volume = audio;
    }
    public float GetAudio()
    {
        return volumen;
    }
    public void SetVelocidad(float velocidad2)
    {
        velocidad = velocidad2;
    }
    public float GetVelocidad()
    {
        return velocidad;
    }
}
