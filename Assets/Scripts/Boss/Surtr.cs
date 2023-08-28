using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class Surtr : Boss
{
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private Transform surtrSword;
    [SerializeField] private List<Sprite> surtrSpriteList;
    [SerializeField] private float swordSpeed = 10f;
    [SerializeField] private float swordRotationSpeed = 5f;
    [SerializeField] private List<GameObject> effectPrefabs;
    
    private float distance = 0f;
    private float stopTime = 0f;
    private float stopCoolTime = 0f;
    private bool isFollow = true;
    CSVReader bookDB;

    private void Start()
    {
        bookDB = BookManager.Instance.bookDB;
        playerController = player.GetComponent<PlayerController>();
        pattern += UseAPattern;
        cooltime = Random.Range(1.2f, 2f);
        currentGauge = 0f;
        isBusy = false;
    }

    private Vector3 _vel;
    private float smoothTime = 0.5f;
    private void Update()
    {
        // Sword Control
        if (isFollow)
        {
            Vector3 lookDirection = (player.transform.position - surtrSword.position).normalized;
            var swordLookatAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            surtrSword.rotation = Quaternion.Slerp(surtrSword.rotation, Quaternion.Euler(new Vector3(0, 0, swordLookatAngle)), swordRotationSpeed * Time.deltaTime);

            var offset = (player.transform.position - surtrSword.position).normalized;
            surtrSword.position = Vector3.SmoothDamp(surtrSword.position, player.transform.position - offset * 6f, ref _vel, smoothTime);
        }

        ////µð¹ö±×
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Debug.Log("1 " + isBusy);
        //    StartCoroutine(Pattern1());
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Debug.Log("2 " + isBusy);
        //    StartCoroutine(Pattern2());
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Debug.Log("3 " + isBusy);
        //    StartCoroutine(Pattern3());
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    Debug.Log("4 " + isBusy);
        //    StartCoroutine(Pattern4());
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    Debug.Log("5 " + isBusy);
        //    StartCoroutine(Pattern5());
        //}

        if (!isBusy)
        {
            currentGauge += Time.deltaTime;
            if (currentGauge >= cooltime)
            {
                currentGauge = 0f;
                cooltime = Random.Range(1f, 1.5f);
                pattern();
            }
        }
    }

    public void UseAPattern()
    {
        Debug.LogWarning("UseAPattern");
        int i = Random.Range(0, 5);
        switch (i)
        {
            case 0:
                StartCoroutine(Pattern1());
                break;
            case 1:
                StartCoroutine(Pattern2());
                break;
            case 2:
                StartCoroutine(Pattern3());
                break;
            case 3:
                StartCoroutine(Pattern4());
                break;
            case 4:
                StartCoroutine(Pattern5());
                break;
        }
    }

    // ºÒÀÇ Âü°Ý
    public IEnumerator Pattern1()
    {
        if (isBusy) yield break;

        isBusy = true;
        isFollow = false;
        string skill = "Surtr1";
        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        float originalRotation = Mathf.Atan2(player.transform.position.y - surtrSword.position.y, player.transform.position.x - surtrSword.position.x) * 180 / Mathf.PI;
        var rotateTween = surtrSword.DORotate(new Vector3(0, 0, originalRotation), 0.5f);

        //Sequence swordSequence = DOTween.Sequence();
        //swordSequence.Append(surtrSword.DORotate(new Vector3(0, 0, originalRotation - 120f), 3f).SetEase(Ease.InSine));
        //swordSequence.Append(surtrSword.DORotate(new Vector3(0, 0, originalRotation), 1f));
        //swordSequence.Append(surtrSword.DORotate(new Vector3(0, 0, originalRotation + 120f), 1f));

        // surtrSword.DORotate(new Vector3(0, 0, originalRotation + 120f), 0.5f);
        yield return rotateTween.WaitForCompletion();
        swordAnimator.SetTrigger("Swipe");

        AudioManager.Instance.PlaySfx(7);
        yield return new WaitForSeconds(0.5f - effect1);
        _Warner.InstantiateHitFan60(surtrSword.position, player.transform.position, 10f);

        yield return new WaitForSeconds(effect1);
        _Warner.RemoveAllHitArea();

        
        Instantiate(effectPrefabs[0], surtrSword.transform.position, surtrSword.rotation);
        Instantiate(effectPrefabs[2], surtrSword.transform.position, surtrSword.rotation);

        isFollow = true;
        yield return new WaitForSeconds(0.5f);
        isBusy = false;

        GameManager.Instance.QuestManager.UpPatternSeeCount(0);
    }
    // ¸¶±×¸¶ ±âµÕ
    public IEnumerator Pattern2()
    {
        if (isBusy) yield break;

        isBusy = true;
        isFollow = false;

        string skill = "Surtr2";
        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        var moveTween = surtrSword.DOMove(new Vector3(0, 6, 0), 1f);
        surtrSword.DORotate(new Vector3(0, 0, -90f), 1f);
        swordAnimator.SetTrigger("SetDown");
        yield return moveTween.WaitForCompletion();
        yield return new WaitForSeconds(0.6f);


        GameObject[] flameList = new GameObject[6];
        AudioManager.Instance.PlaySfx(7);
        yield return new WaitForSeconds(0.5f - effect1);
        for (int i = 0; i < 6; i++)
        {
            flameList[i] = Instantiate(effectPrefabs[4], new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-9.5f, 9.5f), 0), Quaternion.identity);
            _Warner.InstantiateHitCircle(flameList[i].transform.position + new Vector3(0, -2f, 0), 1f);
        }

        
        yield return new WaitForSeconds(effect1);
        swordAnimator.SetTrigger("SetUp");
        _Warner.RemoveAllHitArea();
        yield return new WaitForSeconds(1.5f);
        

        
        for (int i = 0; i < 6; i++)
        {
            Destroy(flameList[i]);
        }
        isFollow = true;
        isBusy = false;

        GameManager.Instance.QuestManager.UpPatternSeeCount(1);
    }

    // ºÒ²É È¸Àü
    public IEnumerator Pattern3()
    {
        if (isBusy) yield break;
        isBusy = true;
        isFollow = false;

        string skill = "Surtr3";
        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        Vector3 cachedPlayerPosition = player.transform.position;
        float originalRotation = Mathf.Atan2(cachedPlayerPosition.y - surtrSword.position.y, cachedPlayerPosition.x - surtrSword.position.x) * 180 / Mathf.PI;
        var rotateTween = surtrSword.DORotate(new Vector3(0, 0, originalRotation), 0.5f);
        yield return rotateTween.WaitForCompletion();

        Instantiate(effectPrefabs[6], surtrSword.position, Quaternion.identity);

        yield return new WaitForSeconds(1f - effect1);
        AudioManager.Instance.PlaySfx(7);
        _Warner.InstantiateHitBox(surtrSword.position, cachedPlayerPosition, 3f);
        yield return new WaitForSeconds(effect1);

        _Warner.RemoveAllHitArea();

        
        var magmaPool = Instantiate(effectPrefabs[5], surtrSword.position + (cachedPlayerPosition - surtrSword.position).normalized * 2f, Quaternion.identity);
        MagmaPool magmaPoolScript = magmaPool.GetComponent<MagmaPool>();
        magmaPoolScript.SetDirection((cachedPlayerPosition - surtrSword.position).normalized);

        yield return new WaitForSeconds(0.3f);

        isBusy = false;
        isFollow = true;
        yield return null;

        GameManager.Instance.QuestManager.UpPatternSeeCount(2);
    }

    // ÀÛ¿­ ±¤¼±
    public IEnumerator Pattern4()
    {
        if (isBusy) yield break;
        isBusy = true;
        isFollow = false;

        string skill = "Surtr4";
        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        Vector3 cachedPlayerPosition = player.transform.position;
        float originalRotation = Mathf.Atan2(cachedPlayerPosition.y - surtrSword.position.y, cachedPlayerPosition.x - surtrSword.position.x) * 180 / Mathf.PI;
        var rotateTween = surtrSword.DORotate(new Vector3(0, 0, originalRotation), 0.5f);
        yield return rotateTween.WaitForCompletion();

        AudioManager.Instance.PlaySfx(7);
        yield return new WaitForSeconds(0.5f - effect1);
        _Warner.InstantiateHitBox(surtrSword.position, cachedPlayerPosition, 1f);
        Instantiate(effectPrefabs[6], surtrSword.position, Quaternion.identity);
        yield return new WaitForSeconds(effect1);

        _Warner.RemoveAllHitArea();

        
        Instantiate(effectPrefabs[7], surtrSword.position, surtrSword.rotation);

        yield return new WaitForSeconds(0.2f);

        isBusy = false;
        isFollow = true;
        yield return null;

        GameManager.Instance.QuestManager.UpPatternSeeCount(3);
    }

    // È­¿° ÆøÇ³
    public IEnumerator Pattern5()
    {
        if (isBusy) yield break;
        isBusy = true;
        isFollow = false;

        string skill = "Surtr4";
        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        Vector3 cachedPlayerPosition = player.transform.position;

        var moveTween = surtrSword.DOMove(cachedPlayerPosition + new Vector3(0, 2f, 0), 1.5f);
        var rotateTween = surtrSword.DORotate(new Vector3(0, 0, -90), 0.5f);
        yield return moveTween.WaitForCompletion();
        swordAnimator.SetTrigger("SetDown");

        yield return new WaitForSeconds(0.5f - effect1);
        AudioManager.Instance.PlaySfx(7);
        _Warner.InstantiateHitCircle(surtrSword.position + new Vector3(0, -2f, 0), 8f);
        Instantiate(effectPrefabs[6], surtrSword.position + new Vector3(0, -2f, 0), Quaternion.identity);
        yield return new WaitForSeconds(effect1);

        Instantiate(effectPrefabs[8], surtrSword.position + new Vector3(0, -2f, 0), Quaternion.identity);
        

        _Warner.RemoveAllHitArea();
        swordAnimator.SetTrigger("SetUp");

        yield return new WaitForSeconds(1f);

        isBusy = false;
        isFollow = true;
        yield return null;
    }
}
