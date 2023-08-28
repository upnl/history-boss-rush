using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    #region Internal
    private float maxHP = 100f;
    private float nowHP = 100f;
    private float normalDamage = 2f;

    [SerializeField] protected Warner _Warner;
    [SerializeField] protected LayerMask _playerLayer;
    #endregion

    #region External
    public float MaxHP => maxHP;
    public float NowHP => nowHP;

    public string bossName;
    #endregion


    [SerializeField] Slider hpSlider;
    
    public GameObject player;
    protected PlayerController playerController;
    [SerializeField] protected Collider2D playerCollider;

    protected bool isBusy = true;
    protected float cooltime = 2f;
    protected float currentGauge = 0f;

    protected delegate void Pattern();
    protected Pattern pattern;

    public virtual void GetDamaged()
    {
        int historyLevel = BookManager.Instance.CheckBookEquipped("Challenge");

        float effect1 = float.Parse(BookManager.Instance.bookDB.GetData().Find(
            e => e[BookManager.Instance.bookDB.GetHeaderIndex("title")].Equals("Challenge") &&
            int.Parse(e[BookManager.Instance.bookDB.GetHeaderIndex("level")]) == historyLevel)[BookManager.Instance.bookDB.GetHeaderIndex("effect1")]);

        nowHP -= normalDamage * effect1 / 100f * 1.5f;
        BookManager.Instance.AddBlood(10);
        hpSlider.value = nowHP/maxHP;
        GameManager.Instance.QuestManager.CheckAttackPercent();
        if (nowHP <= 0f)
        {
            BookManager.Instance.BossDefeated(bossName);
            GameManager.Instance.GameStateManager.Win();
        }
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if(col.gameObject.tag == "Slash")
    //    {
    //        GetDamaged();
    //    }
    //}
}
