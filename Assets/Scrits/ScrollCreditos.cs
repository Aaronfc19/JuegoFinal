using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    public float velocidadScroll = 30f; // píxeles por segundo
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, velocidadScroll * Time.deltaTime);
    }
}