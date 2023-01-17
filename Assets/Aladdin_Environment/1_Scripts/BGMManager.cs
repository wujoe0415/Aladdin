using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public Trigger ice2desert;
    public Trigger desert2forest;

    public AudioSource ice;
    public AudioSource desert;
    public AudioSource forest;

    float fadeOutTime = 3;

    enum State { Ice, Ice2Desert, Desert, Desert2Forest, Forest}
    State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Ice;
        ice.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Ice)
        {
            if (ice2desert.GetTrigger())
            {
                state = State.Ice2Desert;
            }
        }
        else if(state == State.Ice2Desert)
        {
            ice.volume -= Time.deltaTime / 5 / fadeOutTime;
            if(ice.volume <= 0)
            {
                state = State.Desert;
                ice.Stop();
                desert.Play();
            }
        }
        else if(state == State.Desert)
        {
            if (desert2forest.GetTrigger())
            {
                state = State.Desert2Forest;
            }
        }
        else if (state == State.Desert2Forest)
        {
            desert.volume -= Time.deltaTime / 20 / fadeOutTime;
            if (desert.volume <= 0)
            {
                state = State.Forest;
                desert.Stop();
                forest.Play();
            }
        }
    }
}
