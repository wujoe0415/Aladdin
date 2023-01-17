using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneFly : MonoBehaviour
{
    public InputActionProperty FrontTracker;
    public InputActionProperty BackTracker;
    public InputActionProperty LeftTracker;
    public InputActionProperty RightTracker;

    public InputActionProperty[] TrackerStates;
    public Fly fly;
    public Recenter recenter;
    private bool hasInited = false;

    private float fbThreshold = 0.045f;
    private float lrThreshold = 0.07f;

    private float initFB = 0f;

    [Header("UI")]
    [Tooltip("If has not inited, show this UI")]
    public GameObject OpeningUI;
    [Tooltip("If has inited and the tracker lost, show this UI")]
    public GameObject LostTrackedUI;
    private void OnEnable()
    {
        hasInited = false;
        OpeningUI.SetActive(true);
        LostTrackedUI.SetActive(false);
    }
    void Update()
    {
        if(!isTracked() && hasInited)
        {
            LostTrackedUI.SetActive(true);
            fly.OnSlideIdle();
            fly.OnTurnIdle();
            return;
        }
        if (!isTracked())
            return;
        if (!hasInited)
        {
            OpeningUI.SetActive(false);
            recenter.RecenterTransform();
            initFB = FrontTracker.action.ReadValue<Vector3>().y - BackTracker.action.ReadValue<Vector3>().y;
            hasInited = true;
            return;
        }
        LostTrackedUI.SetActive(false);
        if (isFront())
            fly.OnTakeOff();
        else if (isBack())
            fly.OnLanding();
        else
            fly.OnSlideIdle();

        if (isLeft())
            fly.OnTurnLeft();
        else if (isRight())
            fly.OnTurnRight();
        else
            fly.OnTurnIdle();
    }
    private bool isTracked()
    {
        bool allReady = true;
        foreach (var state in TrackerStates)
        {
            allReady = (state.action.ReadValue<int>() != 0);
            if (!allReady)
                break;

        }
        return allReady;
    }
    private bool isFront()
    {
        if (FrontTracker.action.ReadValue<Vector3>().y - BackTracker.action.ReadValue<Vector3>().y > fbThreshold + initFB)
            return true;
        else
            return false;
    }
    private bool isBack()
    {
        if (BackTracker.action.ReadValue<Vector3>().y - FrontTracker.action.ReadValue<Vector3>().y > fbThreshold - initFB)
            return true;
        else
            return false;
    }
    private bool isRight()
    {
        if (RightTracker.action.ReadValue<Vector3>().y - LeftTracker.action.ReadValue<Vector3>().y > lrThreshold)
            return true;
        else
            return false;
    }
    private bool isLeft()
    {
        if (LeftTracker.action.ReadValue<Vector3>().y - RightTracker.action.ReadValue<Vector3>().y > lrThreshold)
            return true;
        else
            return false;
    }
}
