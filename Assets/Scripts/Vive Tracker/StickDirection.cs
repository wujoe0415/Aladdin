using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickDirection : MonoBehaviour
{
    public Vector3 direction = new Vector3(0f, 0f, 0f);
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, direction, Color.red);
    }
}
