using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool triggered = false;
    public FogFade Fade;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
            Fade.FadeToFog();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Fade.FadeOutFog();
        }
    }

    public bool GetTrigger()
    {
        bool re_trigger = triggered;
        triggered = false;
        return re_trigger;
    }
}
