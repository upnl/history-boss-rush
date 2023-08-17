using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _col;
    [SerializeField] private LayerMask CollisionLayer;
    private float _bulletSpeed = 10f;
    private float _totalTime;

    private void Update()
    {
        _totalTime += Time.deltaTime;
        ApplyMovement();
        HandleCollisions();
    }

    private void ApplyMovement()
    {
        transform.position += transform.right * Time.deltaTime * _bulletSpeed;
    }
    
    private void HandleCollisions()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(_col.bounds.center, _col.bounds.extents, _col.transform.eulerAngles.z, CollisionLayer);

        foreach (Collider2D collider in collisions)
        {
            Debug.Log(collider.transform.name);
            if (collider.transform.root == transform.root)
                continue;

            var objectController = collider.transform.root.GetComponent<Boss>();
            if (objectController != null)
            {
                objectController.GetDamaged();
            }
        }

        if (collisions.Length > 0)
        {
            Destroy(gameObject);
        }
    }
}