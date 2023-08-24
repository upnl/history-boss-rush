using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSlashEmbers : MonoBehaviour
{
    [SerializeField] private GameObject emberPrefab;

    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float emberCount = 8f;

    private void Start()
    {
        StartCoroutine(EmberSpawnCoroutine());
    }

    private IEnumerator EmberSpawnCoroutine()
    {
        float originalRotation = transform.rotation.eulerAngles.z;
        Debug.Log(originalRotation);
        
        float distance = 0f;

        for (int i = 0; i < emberCount; i++)
        {
            distance = maxDistance / emberCount * (i + 1);

            for (int j = 0; j < 5; j++)
            {
                float targetRotation = originalRotation + (j - 2) * 30f;
                Vector3 direction = new Vector3(Mathf.Cos(targetRotation * Mathf.Deg2Rad), Mathf.Sin(targetRotation * Mathf.Deg2Rad));
                Instantiate(emberPrefab, transform.position + distance * (direction), Quaternion.identity);
            }
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;

    }
}
