using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.Rendering.PostProcessing;

public enum Dir { up, down, left, right, NULL}
public class Fly : MonoBehaviour
{
    public float DriveSpeed = 0f;

    [Range(0f, 100f)]
    public float MaxDriveSpeed = 100f;
    [Range(0f, 90f)]
    public float DriveAcceleration = 4.9f;
    [Range(0f, 5f)]
    public float TiltAngle = 0.5f;
    [Range(0f, 5f)]
    public float PitchAngle = 0.5f;
    [Range(0f, 60f)]
    public float MaxPitchAngle = 30f;

    //public ViveTracker LeftFoot;
    //public ViveTracker LeftKnee;
    //public ViveTracker RightFoot;
    //public ViveTracker RightKnee;

    private Quaternion idle;
    private Quaternion leftIdle;
    private Quaternion rightIdle;
    //private bool hasInited = false;

    public Transform lefHandler;
    public Transform rightHadler;

    public AdjustFov fov;
    private void Awake()
    {
        idle = transform.rotation;
    }
    private void Update()
    {
        OnForward();
        //if (Input.GetKey(KeyCode.LeftArrow))
        //    OnTurnLeft();
        //else if (Input.GetKey(KeyCode.RightArrow))
        //    OnTurnRight();
        //else
        //    OnTurnIdle();
        //if (Input.GetKey(KeyCode.UpArrow))
        //    OnTakeOff();
        //else if (Input.GetKey(KeyCode.DownArrow))
        //    OnLanding();
        //else
        //    OnSlideIdle();
    }
    private void MoveAccelerate()
    {
        DriveSpeed = Mathf.Clamp(DriveSpeed + DriveAcceleration * Time.deltaTime /* Input.GetAxis("Vertical")*/, -1f * MaxDriveSpeed, MaxDriveSpeed);
        if (DriveSpeed != 0)
            DriveSpeed = Friction.FrictionOperation(DriveSpeed);
    }

    private void Move()
    {
        float distance = DriveSpeed * Time.deltaTime;
        Vector3 faceForward = transform.TransformDirection(Vector3.forward);

        this.gameObject.transform.position += faceForward * distance;
    }
    private void Rotate()
    {
        if (Input.GetAxis("Horizontal") != 0f && Mathf.Abs(DriveSpeed) > 0f)
            DriveSpeed = Friction.TiltFrictionOperation(DriveSpeed);
        if (DriveSpeed == 0f)
            return;

        float tiltAroundY = Input.GetAxis("Horizontal") * TiltAngle;
        // Vector3.up is y axis
        this.gameObject.transform.Rotate(0f, tiltAroundY, 0f, Space.Self);

    }
    public void OnForward()
    {
        MoveAccelerate();
        Move();
    }
    private void OnSlide(Dir dir)
    {
        if (dir == Dir.up)
            OnTakeOff();
        else if (dir == Dir.down)
            OnLanding();
        //else
        //    OnSlideIdle();
    }
    public void OnTakeOff()
    {
        Debug.Log("TakeOff");
        if (Mathf.Abs(DriveSpeed) > 0f)
            DriveSpeed = Friction.TiltFrictionOperation(DriveSpeed);
        if (DriveSpeed == 0f) return;

        float limit = Mathf.Clamp(transform.localRotation.eulerAngles.x - PitchAngle, MaxPitchAngle, 360 - MaxPitchAngle);
        if (limit == transform.localRotation.eulerAngles.x - PitchAngle)
            return;

        transform.Rotate(-PitchAngle, 0f, 0f, Space.Self);
        fov.UpdateFovY(0.375f);
    }
    public void OnLanding()
    {
        Debug.Log("Landing");
        if (Mathf.Abs(DriveSpeed) > 0f)
            DriveSpeed = Friction.TiltFrictionOperation(DriveSpeed);
        if (DriveSpeed == 0f) return;

        float limit = Mathf.Clamp(transform.localRotation.eulerAngles.x + PitchAngle, MaxPitchAngle, 360 - MaxPitchAngle);
        if (limit == transform.localRotation.eulerAngles.x + PitchAngle)
        {
            Debug.Log("Limit");
            return;
        }
        transform.Rotate(PitchAngle, 0f, 0f, Space.Self);
        fov.UpdateFovY(0.625f);
    }
    public void OnSlideIdle()
    {
        float angle = transform.rotation.eulerAngles.x - idle.eulerAngles.x;
        if (Mathf.Abs(angle) > 0.5)
            transform.Rotate((angle - 180)/ Mathf.Abs(angle - 180) * PitchAngle * 0.15f, 0f, 0f, Space.Self);
        else
        {
            var temp = transform.rotation.eulerAngles;
            temp.x = idle.eulerAngles.x;
            transform.rotation = Quaternion.Euler(temp);
        }
        fov.UpdateFovY(0.5f);
    }
    private void OnTurn(Dir dir)
    {
        if (dir == Dir.left)
            OnTurnLeft();
        else if (dir == Dir.right)
            OnTurnRight();
        else
            OnTurnRight();
            
    }
    public void OnTurnRight()
    {
        Debug.Log("Right");
        if (Mathf.Abs(DriveSpeed) > 0f)
            DriveSpeed = Friction.TiltFrictionOperation(DriveSpeed);
        if (DriveSpeed == 0f) return;

        float tiltAroundY = TiltAngle;
        // Vector3.up is y axis
        this.gameObject.transform.Rotate(0f, tiltAroundY, 0f, Space.World);
        fov.UpdateFovX(0.3f);
    }
    public void OnTurnLeft()
    {
        Debug.Log("Left");
        if (Mathf.Abs(DriveSpeed) > 0f)
            DriveSpeed = Friction.TiltFrictionOperation(DriveSpeed);
        if (DriveSpeed == 0f) return;

        float tiltAroundY = -TiltAngle;
        // Vector3.up is y axis
        this.gameObject.transform.Rotate(0f, tiltAroundY, 0f, Space.World);
        fov.UpdateFovX(0.7f);
    }
    public void OnTurnIdle()
    {
        fov.UpdateFovX(0.5f);
    }
}