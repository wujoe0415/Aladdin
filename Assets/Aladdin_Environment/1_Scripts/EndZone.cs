using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    public FogFade Fade;
    public GameObject EndUI;

    private void Awake()
    {
        EndUI.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EndUI.SetActive(true);
            Invoke("FadeEnd", 3.5f);
        }
    }
    private void FadeEnd()
    {
        Fade.FadeToFog();
    }
}
