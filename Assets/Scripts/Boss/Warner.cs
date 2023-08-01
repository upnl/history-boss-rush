using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warner : MonoBehaviour
{
    [SerializeField] private GameObject hitBoxPrefab;
    [SerializeField] private GameObject hitBoxCenterPrefab;
    [SerializeField] private GameObject hitCirclePrefab;
    [SerializeField] private GameObject hitFan60Prefab;
    [SerializeField] private GameObject hitFan120Prefab;

    //public GameObject hitAreaParent;
    
    public GameObject InstantiateHitBox(Vector3 origin, Vector3 destination, float width = 1f, float length = 28.4f)
    {
        GameObject hit = Instantiate(hitBoxPrefab, transform);
        hit.transform.localScale = new Vector3(width, length, 1f);
        hit.transform.localPosition = origin;
        Vector3 v = destination - origin;
        hit.transform.localRotation = Quaternion.Euler(0f, 0f, 270f + Mathf.Atan2(v.y, v.x) / Mathf.PI * 180f);
        return hit;
    }

    public GameObject InstantiateHitBoxInCenter(Vector3 center, float degree, float width = 1f)
    {
        GameObject hit = Instantiate(hitBoxCenterPrefab, transform);
        hit.transform.localScale = new Vector3(width, hit.transform.localScale.y, 1f);
        hit.transform.localPosition = center;
        hit.transform.localRotation = Quaternion.Euler(0f, 0f, degree);
        // degree가 0이면 세로로 길게 배치
        return hit;
    }

    public GameObject InstantiateHitCircle(Vector3 center, float radius, bool ongoingAttack = false)
    {
        GameObject hit = Instantiate(hitCirclePrefab, transform);
        hit.transform.localScale = new Vector3(radius * 2, radius * 2, 1f);
        hit.transform.localPosition = center;
        if (ongoingAttack)
        {
            hit.GetComponent<SpriteRenderer>().color = new Color(0.333333f, 0f, 1f, 0.6627451f);
        }
        return hit;
    }

    public GameObject InstantiateHitFan60(Vector3 center, Vector3 destination, float radius)
    {
        GameObject hit = Instantiate(hitFan60Prefab, transform);
        hit.transform.localScale = new Vector3(radius * 2, radius * 2, 1f);
        hit.transform.localPosition = center;
        Vector3 v = destination - center;
        hit.transform.localRotation = Quaternion.Euler(0f, 0f, 270f + Mathf.Atan2(v.y, v.x) / Mathf.PI * 180f);
        return hit;
    }

    public void RemoveAllHitArea()
    {
        foreach (Transform t in GetComponentInChildren<Transform>())
        {
            Destroy(t.gameObject);
        }
    }
}
