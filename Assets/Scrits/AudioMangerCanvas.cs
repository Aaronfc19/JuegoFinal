using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMangerCanvas : MonoBehaviour
{
    [SerializeField] private Slider volumenSlider;
    [SerializeField] private Slider velocidadSlider;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }

        // Initialize sliders with current audio settings
        volumenSlider.value = audioManager.GetAudio();
        velocidadSlider.value = audioManager.GetVelocidad();

        // Add listeners to sliders
        volumenSlider.onValueChanged.AddListener(SetAudio);
        velocidadSlider.onValueChanged.AddListener(SetVelocidad);

    }
    public void SetAudio(float value)
    {
        audioManager.SetAudio(value);
    }
    public void SetVelocidad(float value)
    {
        audioManager.SetVelocidad(value);
    }

}
