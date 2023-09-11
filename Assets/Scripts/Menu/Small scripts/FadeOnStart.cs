using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]

public class FadeOnStart : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float startApha, endAlpha, fadeSpeed;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = startApha;

        if (startApha > endAlpha && fadeSpeed > 0) // Fade Out negative fadeSpeed correction
            fadeSpeed = -fadeSpeed;
    }

    private void Update()
    {
        if (canvasGroup.alpha >= 0 && canvasGroup.alpha <= 1)
            canvasGroup.alpha += fadeSpeed*Time.deltaTime;
    }
}
