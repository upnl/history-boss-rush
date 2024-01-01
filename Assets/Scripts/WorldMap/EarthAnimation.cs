using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAnimation : MonoBehaviour
{
    private Animator animator;
    private int before = 0;
    private int current = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (current != 2)
            {
                before = current;
                current++;
                animator.SetInteger("before", before);
                animator.SetInteger("current", current);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (current != 0)
            {
                before = current;
                current--;
                animator.SetInteger("before", before);
                animator.SetInteger("current", current);
            }
        }
    }
}
