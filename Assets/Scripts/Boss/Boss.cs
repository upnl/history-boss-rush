using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Boss : MonoBehaviour
{
    #region Internal
    private float maxHP = 100f;
    private float nowHP = 100f;
    private float normalDamage = 3f;
    #endregion

    #region External
    public float MaxHP => maxHP;
    public float NowHP => nowHP;
    #endregion


    [SerializeField] Slider hpSlider;

    public GameObject hitBoxPrefab;
    public GameObject hitBoxCenterPrefab;
    public GameObject hitCirclePrefab;
    public GameObject hitFan60Prefab;
    public GameObject hitFan120Prefab;

    public GameObject hitAreaParent;

    public GameObject player;
    protected PlayerBehaviour playerBehaviour;
    protected Collider2D playerCollider;

    protected bool isBusy = true;
    protected float cooltime = 2f;
    protected float currentGauge = 0f;

    protected delegate void Pattern();
    protected Pattern pattern;
    public void GetDamaged()
    {

        int historyLevel = BookManager.Instance.CheckBookEquipped("Challenge");

        float effect1 = float.Parse(BookManager.Instance.bookDB.GetData().Find(
            e => e[BookManager.Instance.bookDB.GetHeaderIndex("title")].Equals("Challenge") &&
            int.Parse(e[BookManager.Instance.bookDB.GetHeaderIndex("level")]) == historyLevel)[BookManager.Instance.bookDB.GetHeaderIndex("effect1")]);

        nowHP -= normalDamage * effect1 / 100f;
        BookManager.Instance.AddBlood(10);
        hpSlider.value = nowHP/maxHP;
        GameManager.Instance.QuestManager.CheckAttackPercent();
        if (nowHP <= 0f)
        {
            GameManager.Instance.GameStateManager.Win();
        }
    }

    protected GameObject InstantiateHitBox(Vector3 origin, Vector3 destination, float width = 1f, float length = 28.4f)
    {
        GameObject hit = Instantiate(hitBoxPrefab, hitAreaParent.transform);
        hit.transform.localScale = new Vector3(width, length, 1f);
        hit.transform.localPosition = origin;
        Vector3 v = destination - origin;
        hit.transform.localRotation = Quaternion.Euler(0f, 0f, 270f + Mathf.Atan2(v.y, v.x) / Mathf.PI * 180f);
        return hit;
    }

    protected GameObject InstantiateHitBoxInCenter(Vector3 center, float degree, float width = 1f)
    {
        GameObject hit = Instantiate(hitBoxCenterPrefab, hitAreaParent.transform);
        hit.transform.localScale = new Vector3(width, hit.transform.localScale.y, 1f);
        hit.transform.localPosition = center;
        hit.transform.localRotation = Quaternion.Euler(0f, 0f, degree);
        // degree가 0이면 세로로 길게 배치
        return hit;
    }

    protected GameObject InstantiateHitCircle(Vector3 center, float radius, bool ongoingAttack = false)
    {
        GameObject hit = Instantiate(hitCirclePrefab, hitAreaParent.transform);
        hit.transform.localScale = new Vector3(radius * 2, radius * 2, 1f);
        hit.transform.localPosition = center;
        if (ongoingAttack)
        {
            hit.GetComponent<SpriteRenderer>().color = new Color(0.333333f, 0f, 1f, 0.6627451f);
        }
        return hit;
    }

    protected GameObject InstantiateHitFan60(Vector3 center, Vector3 destination, float radius)
    {
        GameObject hit = Instantiate(hitFan60Prefab, hitAreaParent.transform);
        hit.transform.localScale = new Vector3(radius * 2, radius * 2, 1f);
        hit.transform.localPosition = center;
        Vector3 v = destination - center;
        hit.transform.localRotation = Quaternion.Euler(0f, 0f, 270f + Mathf.Atan2(v.y, v.x) / Mathf.PI * 180f);
        return hit;
    }

    protected void AttackOnAllHitArea()
    {
        foreach (Transform t in hitAreaParent.GetComponentInChildren<Transform>())
        {
            if (t.GetComponent<Collider2D>().IsTouching(playerCollider))
            {
                playerBehaviour.GetDamaged();
            }
        }
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
            GetDamaged();
        }
    }
}
