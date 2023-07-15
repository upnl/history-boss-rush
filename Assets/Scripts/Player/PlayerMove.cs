using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float playerSpeed = 5f;
    private float dashDistance = 10f;
    private float dashTime = 0.5f;
    private Vector3 nowPlayerDirecton = new Vector3(0f, 1f, 0f);
    public bool isdash = false;
    public bool canDash = true;
    private float rollingCoolTime = 3f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            if (GameManager.Instance.GameStateManager.dashTemp)
            {
                GameManager.Instance.GameStateManager.dashTemp = false;
            }
            else
            {
                float hValue = Input.GetAxisRaw("Horizontal");
                float vValue = Input.GetAxisRaw("Vertical");
                if (hValue != 0 || vValue != 0)
                {
                    nowPlayerDirecton = new Vector3(hValue, vValue, 0f).normalized;
                }
                isdash = true;
                canDash = false;
                StartCoroutine(Dash());
            }
        }
        else if (!isdash)
        {
            float hValue = Input.GetAxisRaw("Horizontal");
            float vValue = Input.GetAxisRaw("Vertical");
            if (hValue != 0 || vValue != 0)
            {
                nowPlayerDirecton = new Vector3(hValue, vValue, 0f).normalized;
            }
            var posX = transform.position.x + playerSpeed * hValue * Time.deltaTime;
            var posY = transform.position.y + playerSpeed * vValue * Time.deltaTime;

            transform.position = new Vector3(posX, posY, -1f);
        }
    }

    private IEnumerator Dash()
    {
        float elapsedTime = 0f;
        while (elapsedTime < dashTime)
        {
            transform.position = transform.position + nowPlayerDirecton * dashDistance * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isdash = false;
        yield return new WaitForSeconds(rollingCoolTime - dashTime);
        canDash = true;
    }
}
