using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrackRotation : MonoBehaviour
{
    public Transform Anchor;
    public float o = -90f;
    private void Update()
    {
        Vector3 newRot = Anchor.localRotation.eulerAngles;
        newRot.y = o;
        transform.localRotation = Quaternion.Euler(newRot);
    }
}
