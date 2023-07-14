using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float playerSpeed = 5f;
    // Update is called once per frame
    void Update()
    {
        float hValue = Input.GetAxisRaw("Horizontal");
        float vValue = Input.GetAxisRaw("Vertical");
        var posX = transform.position.x + playerSpeed * hValue * Time.deltaTime;
        var posY = transform.position.y + playerSpeed * vValue * Time.deltaTime;

        transform.position = new Vector3(posX, posY, -1f);
    }
}
