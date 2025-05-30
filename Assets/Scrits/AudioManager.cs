using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of AudioManager exists
    public static AudioManager instance;
    [SerializeField] private float volumen = 0.5f;
    [SerializeField] private float velocidad = 1f;
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource component
    [SerializeField] private AudioClip[] audioClips; // Array to hold audio clips if needed
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
     
        SetVelocidad(velocidad);
        if (PlayerPrefs.HasKey("AudioVolume"))
        {
            volumen = PlayerPrefs.GetFloat("AudioVolume");
            AudioListener.volume = volumen;
        }
        else
        {
            SetAudio(volumen);
            PlayerPrefs.SetFloat("AudioVolume", volumen);
        }
        //En el estar se reproduce el audio clip 0
        if (audioSource != null && audioClips.Length > 0)
        {
            audioSource.clip = audioClips[0]; // Set the first audio clip
            audioSource.loop = true; // Loop the audio
            audioSource.volume = volumen; // Set initial volume
            audioSource.Play(); // Start playing the audio
        }
    }
    // Method to play a specific audio clip
    public void PlayAudioClip(int index)
    {
        if (audioSource != null && audioClips.Length > index && index >= 0)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip index out of range or AudioSource not set.");
        }
    }
    public void SetAudio(float audio)
    {
        AudioListener.volume = audio;
        volumen = audio;
        PlayerPrefs.SetFloat("AudioVolume", volumen); // Save volume to PlayerPrefs
    }
    public float GetAudio()
    {
        return volumen;
    }
    public void SetVelocidad(float velocidad2)
    {
        velocidad = velocidad2;
        Time.timeScale = velocidad; // Set the time scale for game speed
    }
    public float GetVelocidad()
    {
        return velocidad;
    }

}
