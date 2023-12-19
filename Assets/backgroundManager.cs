using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class backgroundManager : MonoBehaviour
{
    [SerializeField] Image[] backgrounds; 
    [SerializeField] private float fadeDuration = 2f;
    public int currentBackgroundIndex = 0;

    

    void Start()
    {
        foreach (var bg in backgrounds)
        {
            bg.canvasRenderer.SetAlpha(0f);
        }
        backgrounds[0].canvasRenderer.SetAlpha(1f);
    }

    public void TransitionToNextBackground()
    {
        StartCoroutine(FadeBackgrounds(currentBackgroundIndex, (currentBackgroundIndex + 1) % backgrounds.Length));
        currentBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Length;
    }

    private IEnumerator FadeBackgrounds(int fadeOutIndex, int fadeInIndex)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            backgrounds[fadeOutIndex].canvasRenderer.SetAlpha(1 - timer / fadeDuration);
            backgrounds[fadeInIndex].canvasRenderer.SetAlpha(timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}

