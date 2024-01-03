using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    public float radius = 10.0f;
    public float theta = 0f;
    public float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (theta >= Mathf.PI * 2)
        {
            theta = 0f;
        }
        theta += (0.01f * speed);
        Vector3 position = gameObject.transform.position;
        position.x = Mathf.Cos(theta) * radius;
        position.y = Mathf.Sin(theta) * radius;
        gameObject.transform.position = position;
    }
}
