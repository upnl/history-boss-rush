using System.Collections;
using System.Collections.Generic;
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

    private GameStateManager gameStateManager;

    private void Start()
    {
        gameStateManager = GameManager.Instance.GameStateManager;
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
            transform.position += nowPlayerDirecton * dashConstant * Time.deltaTime * (dashTime - elapsedTime) * (dashTime - elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        IsDash = false;
        yield return new WaitForSeconds(rollingCoolTime - dashTime);
        CanDash = true;
    }
}
