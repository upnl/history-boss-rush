using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAnimation : MonoBehaviour
{
    [SerializeField] GameObject Field;
    [SerializeField] GameObject LeftArrow;
    [SerializeField] GameObject RightArrow;

    public static int Before = 0;
    public static int Current = 0;
    public static bool FieldOpen = false;

    public float EarthStartPosition = 0f;
    public float EarthMovedPosition = -4.5f;
    public float EarthMovingSpeed = 1.0f;

    private Animator _animator;
    private Animator _fieldAnimator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _fieldAnimator = Field.GetComponent<Animator>();
    }

    void Start()
    {

    }
    
    void Update()
    {
        Vector3 position = gameObject.transform.position;
        if (Input.GetKeyDown(KeyCode.RightArrow) && !FieldOpen)
        {
            RightMoving();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !FieldOpen)
        {
            LeftMoving();
        }
        if (FieldOpen && !Field.gameObject.activeSelf)
        {
            if (position.x > EarthMovedPosition)
            {
                position.x = position.x - Time.deltaTime * EarthMovingSpeed;
                gameObject.transform.position = position;
            }
            else
            {
                Field.gameObject.SetActive(true);
            }
        }
        else if (!FieldOpen)
        {
            if (Field.gameObject.activeSelf && _fieldAnimator.GetCurrentAnimatorStateInfo(0).IsName("FieldClose") && 
                _fieldAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f
            )
            {
                Field.gameObject.SetActive(false);
            }
            else if (!Field.gameObject.activeSelf)
            {
                if (position.x < EarthStartPosition)
                {
                    position.x = position.x + Time.deltaTime * EarthMovingSpeed;
                    gameObject.transform.position = position;
                }
                else
                {
                    position.x = 0f;
                    position.y = 0f;
                    gameObject.transform.position = position;
                    gameObject.GetComponent<Animator>().enabled = true;
                    LeftArrow.gameObject.SetActive(true);
                    RightArrow.gameObject.SetActive(true);
                }
            }
        }
    }

    public void RightMoving()
    {
        if (Current != 2)
        {
            Before = Current;
            Current++;
            _animator.SetInteger("before", Before);
            _animator.SetInteger("current", Current);
        }
    }

    public void LeftMoving()
    {
        if (Current != 0)
        {
            Before = Current;
            Current--;
            _animator.SetInteger("before", Before);
            _animator.SetInteger("current", Current);
        }
    }

    public void OnMouseDown()
    {
        if (!FieldOpen)
        {
            FieldOpen = true;
            _fieldAnimator.SetBool("FieldOpen", true);
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
            gameObject.GetComponent<Animator>().enabled = false;
        }
        else
        {
            FieldOpen = false;
            _fieldAnimator.SetBool("FieldOpen", false);
        }
    }

    public void OnMouseOver()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(179 / 255f, 179 / 255f, 179 / 255f);
    }

    public void OnMouseExit()
    {
        if (FieldOpen)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
