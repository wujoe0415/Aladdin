using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
    [Range(0f, 90f)]
    public float FrictionAcceleration;
    [Range(0f, 90f)]
    public float TiltFrivtionAccerleration;

    private static float s_frictionAcceleration;
    private static float s_tiltFrictionAcceleration;

    private void Start()
    {
        s_frictionAcceleration = FrictionAcceleration;
        s_tiltFrictionAcceleration = TiltFrivtionAccerleration;
    }

    public static float FrictionOperation(float DriveSpeed)
    {
        float finalSpeed = DriveSpeed - s_frictionAcceleration * Time.deltaTime * DriveSpeed / Mathf.Abs(DriveSpeed);
        if (DriveSpeed * finalSpeed <= 0f)
            return 0;
        return finalSpeed;
        //DriveSpeed = Mathf.Clamp(Mathf.Abs(DriveSpeed), -1 * MaxDriveSpeed, MaxDriveSpeed);
    }
    public static float TiltFrictionOperation(float DriveSpeed)
    {
        float finalSpeed = DriveSpeed - s_tiltFrictionAcceleration * Time.deltaTime * DriveSpeed / Mathf.Abs(DriveSpeed);
        if (DriveSpeed * finalSpeed <= 0f)
            return 0f;

        return finalSpeed;
        //DriveSpeed = Mathf.Clamp(Mathf.Abs(DriveSpeed), -1 * MaxDriveSpeed, MaxDriveSpeed);
    }
}