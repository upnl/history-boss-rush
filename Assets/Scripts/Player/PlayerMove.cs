using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float playerSpeed = 5f;
    private float dashConstant = 80f;
    private float dashTime = 0.5f;
    private Vector3 nowPlayerDirecton = new Vector3(0f, 1f, 0f);
    [HideInInspector] public bool IsDash = false;
    [HideInInspector] public bool CanDash = true;
    private float rollingCoolTime = 3f;
    [HideInInspector] public bool CanMove = true;
    private Animator animator;

    private GameStateManager gameStateManager;

    private void Start()
    {
        gameStateManager = GameManager.Instance.GameStateManager;
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanDash && CanMove)
        {
            if (gameStateManager.dashTemp)
            {
                gameStateManager.dashTemp = false;
            }
            else
            {
                float hValue = Input.GetAxisRaw("Horizontal");
                float vValue = Input.GetAxisRaw("Vertical");
                if (hValue != 0 || vValue != 0)
                {
                    nowPlayerDirecton = new Vector3(hValue, vValue, 0f).normalized;
                }
                else
                {
                    nowPlayerDirecton = new Vector3(0f, 0f, 0f);
                }
                if (vValue == 0)
                {
                    if(hValue == 1)
                    {
                        animator.SetInteger("WalkInt", 1);
                    }
                    else if(hValue == -1)
                    {
                        animator.SetInteger("WalkInt", 3);
                    }
                }
                else if(vValue == 1)
                {
                    animator.SetInteger("WalkInt", 2);
                }
                else
                {
                    animator.SetInteger("WalkInt", 0);
                }
                IsDash = true;
                CanDash = false;
                StartCoroutine(Dash());
            }
        }
        else if (!IsDash && CanMove)
        {
            float hValue = Input.GetAxisRaw("Horizontal");
            float vValue = Input.GetAxisRaw("Vertical");
            if (hValue != 0 || vValue != 0)
            {
                nowPlayerDirecton = new Vector3(hValue, vValue, 0f).normalized;
            }
            else
            {
                nowPlayerDirecton = new Vector3(0f, 0f, 0f);
            }
            if (vValue == 0)
            {
                if(hValue == 1)
                {
                    animator.SetInteger("WalkInt", 1);
                }
                else if(hValue == -1)
                {
                    animator.SetInteger("WalkInt", 3);
                }
            }
            else if(vValue == 1)
            {
                animator.SetInteger("WalkInt", 2);
            }
            else
            {
                animator.SetInteger("WalkInt", 0);
            }
            hValue = nowPlayerDirecton.x;
            vValue = nowPlayerDirecton.y;
            int speedLvl = BookManager.Instance.CheckBookEquipped("Tenacity");
            var posX = transform.position.x + (playerSpeed + speedLvl*2) * hValue * Time.deltaTime;
            var posY = transform.position.y + (playerSpeed + speedLvl*2) * vValue * Time.deltaTime;

            transform.position = new Vector3(posX, posY, 0f);
        }
    }

    private IEnumerator Dash()
    {
        float elapsedTime = 0f;
        while (elapsedTime < dashTime)
        {
            transform.position += nowPlayerDirecton * dashConstant * Time.deltaTime * (dashTime - elapsedTime) * (dashTime - elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        IsDash = false;
        yield return new WaitForSeconds(rollingCoolTime - dashTime);
        CanDash = true;
    }
}
