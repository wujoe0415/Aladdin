using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpeedline : MonoBehaviour
{
    public GameObject SpeedLine;
    public Camera Camera;
    public Transform Caution;

    private void Update()
    {
        if (Vector3.Angle(Camera.transform.forward, Caution.transform.forward) < 20f)
            Active(true);
        else
            Active(false);
    }
    public void Active(bool setActive)
    {
        SpeedLine.SetActive(setActive);
    }
}
