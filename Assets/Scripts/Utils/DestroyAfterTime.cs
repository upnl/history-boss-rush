using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float destroyTime;

    private void Start()
    {
        totalTime = 0;
    }

    private float totalTime = 0f;
    private void Update()
    {
        totalTime += Time.deltaTime;
        if (destroyTime < totalTime)
        {
            Destroy(gameObject);
        }
    }
}
