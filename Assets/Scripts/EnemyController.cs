using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] targets;
    public Vector3 targetPos;
    [SerializeField] int targetIdx;
    [SerializeField] int targetCount;
    [SerializeField] Vector3 dir, dir_up, dir_right;
    [SerializeField] Vector3 moveDir;
    float timer;
    public float speed;
    [SerializeField] float x_scale, y_scale, z_scale;
    [SerializeField] float hori_scale, vert_scale;

    CharacterController controller;
    bool stop;

    Animator animator;

    public GameObject player;
    void Start()
    {
        targetCount = targets.Length;
        targetIdx = 0;
        targetPos = targets[0].transform.position;
        dir = Vector3.Normalize(targetPos - transform.position) * 10;
        transform.LookAt(transform.position + dir);
        dir_up = Quaternion.AngleAxis(90, transform.right) * dir;
        dir_right = Quaternion.AngleAxis(90, transform.up) * dir;

        timer = 0;
        speed = 1.5f;

        hori_scale = Random.Range(0.05f, 0.2f);
        vert_scale = Random.Range(0.05f, 0.2f);

        controller = GetComponent<CharacterController>();
        stop = false;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z - player.transform.position.z > 300 || targetIdx >= targetCount)
        {
            stop = true;
            animator.SetBool("Goal", true);
            return;
        }
        else
        {
            stop = false;
            animator.SetBool("Goal", false);
        }

        moveDir = dir;
        moveDir += dir_up * hori_scale * Mathf.Sin(timer);
        moveDir += dir_right * vert_scale * Mathf.Cos(timer);

        transform.LookAt(transform.position + moveDir);
        controller.Move(moveDir * speed * Time.deltaTime);
        
        timer += Time.deltaTime;
        if (timer > 2 * Mathf.PI)
        {
            timer = 0;

            hori_scale = Random.Range(0.05f, 0.2f);
            vert_scale = Random.Range(0.05f, 0.2f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == targets[targetIdx].name)
        {
            targetIdx++;
            if(targetIdx >= targetCount)
            {
                stop = true;
                animator.SetBool("Goal", true);
            }
            else
            {
                targetPos = targets[targetIdx].transform.position;
                dir = Vector3.Normalize(targetPos - transform.position) * 10;
                dir_up = Quaternion.AngleAxis(90, transform.right) * dir;
                dir_right = Quaternion.AngleAxis(90, transform.up) * dir;
            }
        }
    }
}
