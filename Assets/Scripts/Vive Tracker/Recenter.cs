using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recenter : MonoBehaviour
{
    public Transform resetTransform;
    public GameObject player;
    public Camera vrCamera;

    public Transform resetAnchor;
    public Transform pole;
    public Transform caution;
    
    [ContextMenu("Recenter")]
    public void RecenterTransform()
    {
        float rotateionAngleY = resetAnchor.rotation.eulerAngles.y - pole.rotation.eulerAngles.y;
        //caution.transform.Rotate(0, rotateionAngleY, 0);
        

        rotateionAngleY = resetTransform.rotation.eulerAngles.y -
                vrCamera.transform.rotation.eulerAngles.y;
        player.transform.Rotate(0, rotateionAngleY, 0);

        Vector3 distanceOffset = resetTransform.position - vrCamera.transform.position;
        distanceOffset.y = 0f;
        player.transform.position += distanceOffset;
    }
}
