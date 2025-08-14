using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour
{
    public float MoveSpeed = 1.5f;
    public float TurnSpeed = 100.0f;
    private Animator anim = null;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }
    private void Update()
    {
        float vert = Input.GetAxis("Vertical"); // -1 .. 1
        float horz = Input.GetAxis("Horizontal");

        if (vert > 0)
        {
            anim.SetBool("IsWalking", true);
            anim.SetFloat("Speed", 1.0f);
        }
        else if (vert < 0)
        {
            anim.SetBool("IsWalking", true);
            anim.SetFloat("Speed", -1.0f);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        this.transform.Translate(0.0f, 0.0f, vert * MoveSpeed * Time.deltaTime);
        this.transform.Rotate(0.0f, horz * TurnSpeed * Time.deltaTime, 0.0f);
    }
}
