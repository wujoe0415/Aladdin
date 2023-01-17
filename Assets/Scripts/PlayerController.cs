using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    public float speed = 10;
    public float sensitivity = 0.5f; // sensitive to direction controll
    float speedUpTimer; // countdown for speedup time
    float speedUpDuration = 1;

    public ParticleSystem speedUpEffect;

    enum State { Idle, Moving }
    [SerializeField] State state;

    Vector3 camDir;
    Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        speedUpTimer = 0;

        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        camDir = Camera.main.transform.forward;
        if(state == State.Idle && Input.GetKeyDown(KeyCode.Space))
        {
            state = State.Moving;
        }
        else if(state == State.Moving)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = State.Idle;
            }

            // speed up
            if(speedUpTimer > 0)
            {
                speed = 30;
                speedUpTimer -= Time.deltaTime;
            }
            else
            {
                speed = 20;
            }

            if (Input.GetMouseButton(0))
            {
                speed *= 2;
            }

            // move
            moveDir = camDir;
            if (Input.GetKey(KeyCode.W))
            {
                moveDir += Camera.main.transform.up * sensitivity;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDir -= Camera.main.transform.up * sensitivity;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDir += Camera.main.transform.right * sensitivity;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveDir -= Camera.main.transform.right * sensitivity;
            }
            controller.Move(moveDir * speed * Time.deltaTime);
        }
    }

    public void speedUp()
    {
        speedUpTimer = speedUpDuration;
        speedUpEffect.Play();
        Debug.Log("Speed up");
    }
}
