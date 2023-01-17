using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoleFly : MonoBehaviour
{
    [System.Serializable]
    public struct PoleData
    {
        public Transform Pole;
        public Transform InitPole;
        public InputActionProperty CheckState;
        public Vector3 InitForward;
        public Vector3 InitUp;
        public Vector3 InitInside;
    }
    public Fly fly;
    public Recenter recenter;
    private bool hasInited = false;

    public PoleData LeftPole;
    public PoleData RightPole;
    private Vector3 CurrentUp;

    private void Update()
    {
        if (LeftPole.CheckState.action.ReadValue<int>() == 0 || RightPole.CheckState.action.ReadValue<int>() == 0)
            return;
        if (!hasInited)
        {
            recenter.RecenterTransform();
            Init();
            hasInited = true;
        }
        PoleChange();
    }
    public void Init()
    {
        GameObject lp = new GameObject("Left Init Pole");
        GameObject rp = new GameObject("Right Init Pole");
        lp.transform.parent = transform;
        rp.transform.parent = transform;
        lp.transform.position = LeftPole.Pole.position;
        lp.transform.rotation = LeftPole.Pole.rotation;
        rp.transform.position = RightPole.Pole.position;
        rp.transform.rotation = RightPole.Pole.rotation;


        LeftPole.InitPole = lp.transform;
        RightPole.InitPole = rp.transform;
        ReadValue();
    }
    private void ReadValue()
    {
        LeftPole.InitForward = LeftPole.InitPole.TransformDirection(Vector3.forward);
        LeftPole.InitUp = LeftPole.InitPole.TransformDirection(Vector3.up);
        LeftPole.InitInside = Vector3.Cross(LeftPole.InitForward, LeftPole.InitUp);

        RightPole.InitForward = RightPole.InitPole.TransformDirection(Vector3.forward);
        RightPole.InitUp = RightPole.InitPole.TransformDirection(Vector3.up);
        RightPole.InitInside = Vector3.Cross(RightPole.InitUp, RightPole.InitForward);
    }
    public void PoleChange()
    {
        ReadValue();
        CurrentUp = LeftPole.Pole.TransformDirection(Vector3.up);
        Vector3 tempLIU = Vector3.ProjectOnPlane(CurrentUp, LeftPole.InitForward);
        Vector3 tempLFU = Vector3.ProjectOnPlane(LeftPole.Pole.TransformDirection(Vector3.right), LeftPole.InitUp);


        CurrentUp = RightPole.Pole.TransformDirection(Vector3.up);
        Vector3 tempRIU = Vector3.ProjectOnPlane(CurrentUp, RightPole.InitForward);
        Vector3 tempRFU = Vector3.ProjectOnPlane(RightPole.Pole.TransformDirection(Vector3.right), RightPole.InitUp);

        float lturn = Vector3.Dot(tempLIU, LeftPole.InitInside) > 0 ? Vector3.Angle(tempLIU, LeftPole.InitInside) : 90;
        float rturn = Vector3.Dot(tempRIU, RightPole.InitInside) > 0 ? Vector3.Angle(tempRIU, RightPole.InitInside) : 90;
        //Debug.Log(lturn + " " + rturn);
        // Turn
        if (lturn < 70f && rturn > lturn)
            fly.OnTurnRight();
        else if (rturn < 70f && lturn > rturn)
            fly.OnTurnLeft();
        //else
        //    fly.OnTurnIdle();

        // Slide
        //Debug.Log(Vector3.Angle(tempLFU, LeftPole.InitForward));
        //Debug.Log(Vector3.Angle(tempRFU, RightPole.InitForward));
        if (Mathf.Abs(90 - Vector3.Angle(tempLFU, LeftPole.InitForward)) > 20f && Mathf.Abs(Vector3.Angle(tempRFU, RightPole.InitForward)) > 20f)
        {
            //Debug.Log((Vector3.Dot(tempLFU, LeftPole.InitForward) > 0) + " " + (Vector3.Dot(tempRFU, RightPole.InitForward) > 0));
            if (Vector3.Dot(tempLFU, LeftPole.InitForward) < 0 && Vector3.Dot(tempRFU, RightPole.InitForward) > 0)
                fly.OnLanding();
            else if (Vector3.Dot(tempLFU, LeftPole.InitForward) > 0 && Vector3.Dot(tempRFU, RightPole.InitForward) < 0)
                fly.OnTakeOff();
            //else
            //    fly.OnSlideIdle();
        }
    }
}
