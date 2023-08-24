using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaPool : MonoBehaviour
{
    private Vector3 direction;
    private float poolSpeed = 1f;
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void Update()
    {
        transform.position += direction * poolSpeed * Time.deltaTime;
    }
}
