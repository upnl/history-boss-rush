using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private bool destroyAfterTime;
    [SerializeField] private float destroyTime;
    [SerializeField] private List<Collider2D> collider2Ds;

    private void Start()
    {
        if (objectToDestroy == null)
        {
            Debug.LogError("ObjectToDestroy is null");
            objectToDestroy = this.gameObject;
        }
        totalTime = 0;
    }

    private float totalTime = 0f;
    private void Update()
    {
        totalTime += Time.deltaTime;
        if (destroyAfterTime && destroyTime < totalTime)
        {
            Destroy(objectToDestroy);
        }
    }

    public void SetColliderOn()
    {
        foreach(var collider in collider2Ds)
        {
            collider.enabled = true;
        }
    }

    public void SetColliderOff()
    {
        foreach (var collider in collider2Ds)
        {
            collider.enabled = false;
        }

    }

    public void DestroyObject()
    {
        Destroy(objectToDestroy);
    }
}
