using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float playerSpeed = 5f;
    private float rollDistance = 10f;
    private float rollTime = 0.5f;
    private Vector3 nowPlayerDirecton = new Vector3(0f, 1f, 0f);
    private bool isRolling = false;
    private bool canRolling = true;
    private float rollingCoolTime = 3f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canRolling)
        {   float hValue = Input.GetAxisRaw("Horizontal");
            float vValue = Input.GetAxisRaw("Vertical");
            if(hValue != 0 || vValue != 0)
            {
                nowPlayerDirecton = new Vector3(hValue, vValue, 0f).normalized;
            }
            isRolling = true;
            canRolling = false;
            StartCoroutine(Roll());
        }
        else if(!isRolling)
        {
            float hValue = Input.GetAxisRaw("Horizontal");
            float vValue = Input.GetAxisRaw("Vertical");
            if(hValue != 0 || vValue != 0)
            {
                nowPlayerDirecton = new Vector3(hValue, vValue, 0f).normalized;
            }
            var posX = transform.position.x + playerSpeed * hValue * Time.deltaTime;
            var posY = transform.position.y + playerSpeed * vValue * Time.deltaTime;

            transform.position = new Vector3(posX, posY, -1f);
        }
    }

    private IEnumerator Roll()
    {
        float elapsedTime = 0f;
        while(elapsedTime < rollTime)
        {
            Debug.Log(nowPlayerDirecton);
            transform.position = transform.position + nowPlayerDirecton * rollDistance * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
        yield return new WaitForSeconds(rollingCoolTime - rollTime);
        canRolling = true;
    }
}
