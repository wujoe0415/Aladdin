using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FogFade : MonoBehaviour
{
    public Image Fog;
    public GameObject Fov;

    [ContextMenu("FogIn")]
    public void FadeToFog()
    {
        StopCoroutine(FadeOut());
        StartCoroutine(FadeIn());
    }
    [ContextMenu("FogOut")]
    public void FadeOutFog()
    {
        StopCoroutine(FadeIn());
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeIn()
    {
        Fov.SetActive(false);
        Color init = Color.white;
        init.a = 0f;
        for (float f = 0; f <= 1f; f+=Time.deltaTime)
        {
            Fog.color = Color.Lerp(init, Color.white, f / 1f);
            yield return null;
        }
        Fog.color = Color.white;
    }
    private IEnumerator FadeOut()
    {
        Color final = Color.white;
        final.a = 0f;
        for (float f = 0; f <= 1f; f += Time.deltaTime)
        {
            Fog.color = Color.Lerp(Color.white, final, f / 1f);
            yield return null;
        }
        Fog.color = final;
        Fov.SetActive(true);
    }
}
