using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    private float maxHP = 100f;
    public float MaxHP => maxHP;
    private float nowHP = 100f;
    public float NowHP => nowHP;
    private float normalDamage = 3f;
    [SerializeField] Slider hpSlider;

    public GameObject hitBoxPrefab;
    public GameObject hitBoxCenterPrefab;
    public GameObject hitCirclePrefab;
    public GameObject hitFan60Prefab;
    public GameObject hitFan120Prefab;

    public GameObject hitAreaParent;

    protected bool isBusy = false;

    private void Start()
    {
        isBusy = false;
    }

    public void GetDamaged(int attackBookLevel)
    {
        nowHP -= normalDamage + attackBookLevel;
        hpSlider.value = nowHP/maxHP;
        if (nowHP <= 0f)
        {
            GameManager.Instance.GameStateManager.Win();
        }
    }

    protected void InstantiateHitBox(Vector3 origin, Vector3 destination, float width = 1f, float length = 28.4f)
    {
        Debug.Log(origin + ", " + destination);
        GameObject hit = Instantiate(hitBoxPrefab, hitAreaParent.transform);
        hit.transform.localScale = new Vector3(width, length, 1f);
        hit.transform.position = origin;
        Vector3 v = destination - origin;
        hit.transform.localRotation = Quaternion.Euler(0f, 0f, 270f + Mathf.Atan2(v.y, v.x) / Mathf.PI * 180f);
    }

    protected void InstantiateHitBoxInCenter(Vector3 center, float degree, float width = 1f)
    {
        GameObject hit = Instantiate(hitBoxCenterPrefab, hitAreaParent.transform);
        hit.transform.localScale = new Vector3(width, hit.transform.localScale.y, 1f);
        hit.transform.position = center;
        hit.transform.localRotation = Quaternion.Euler(0f, 0f, degree);
        // degree�� 0�� �� ���η� ��
    }

    protected void RemoveAllHitArea()
    {
        foreach (Transform t in hitAreaParent.GetComponentInChildren<Transform>())
        {
            Destroy(t.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Slash")
        {
            GetDamaged(BookManager.Instance.CheckBookEquipped("Challenge"));
        }
    }
}
