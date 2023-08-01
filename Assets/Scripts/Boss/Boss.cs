using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    #region Internal
    private float maxHP = 100f;
    private float nowHP = 100f;
    private float normalDamage = 3f;

    [SerializeField] protected Warner _Warner;
    #endregion

    #region External
    public float MaxHP => maxHP;
    public float NowHP => nowHP;
    #endregion


    [SerializeField] Slider hpSlider;
    
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

    protected void AttackOnAllHitArea()
    {
        foreach (Transform t in _Warner.transform.GetComponentInChildren<Transform>())
        {
            if (t.GetComponent<Collider2D>().IsTouching(playerCollider))
            {
                playerBehaviour.GetDamaged();
            }
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
