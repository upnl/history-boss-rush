using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStormEmbers : MonoBehaviour
{
    [SerializeField] private GameObject emberPrefab;

    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float emberCount = 8f;

    private void Start()
    {
        StartCoroutine(EmberSpawnCoroutine());
    }

    private void Update()
    {
        transform.Rotate(0, 0, 60f * Time.deltaTime);
    }

    private IEnumerator EmberSpawnCoroutine()
    {
        float originalRotation = transform.rotation.eulerAngles.z + Random.Range(0, 60);
        Debug.Log(originalRotation);

        float distance = 0f;

        for (int i = 0; i < emberCount; i++)
        {
            distance = maxDistance / emberCount * (emberCount - i);

            for (int j = 0; j < 6; j++)
            {
                float targetRotation = originalRotation + (j) * 60f + 30f * Mathf.Sin((float)i / (float)emberCount * Mathf.PI);
                // Debug.Log(targetRotation);
                Vector3 direction = new Vector3(Mathf.Cos(targetRotation * Mathf.Deg2Rad), Mathf.Sin(targetRotation * Mathf.Deg2Rad));
                var ember = Instantiate(emberPrefab, transform.position + distance * (direction), Quaternion.identity);
                ember.transform.SetParent(transform);
            }
            yield return new WaitForSeconds(0.08f);
        }
        yield return null;

    }
}
