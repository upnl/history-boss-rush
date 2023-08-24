using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class Surtr : Boss
{
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
        cooltime = Random.Range(1.5f, 3f);
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

        //�����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1 " + isBusy);
            StartCoroutine(Pattern1());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2 " + isBusy);
            StartCoroutine(Pattern2());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3 " + isBusy);
            StartCoroutine(Pattern3());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4 " + isBusy);
            StartCoroutine(Pattern4());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("5 " + isBusy);
            StartCoroutine(Pattern5());
        }

        if (!isBusy)
        {
            currentGauge += Time.deltaTime;
            if (currentGauge >= cooltime)
            {
                currentGauge = 0f;
                cooltime = Random.Range(1.5f, 3f);
                pattern();
            }
        }
    }

    public void UseAPattern()
    {
        Debug.LogWarning("UseAPattern");
        int i = Random.Range(3, 4);
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

    // ���� ����
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
        Instantiate(effectPrefabs[0], surtrSword.transform.position, surtrSword.rotation);
        Instantiate(effectPrefabs[2], surtrSword.transform.position, surtrSword.rotation);

        isFollow = true;
        yield return new WaitForSeconds(3f);
        isBusy = false;
    }
    // ���׸� ���
    public IEnumerator Pattern2()
    {
        if (isBusy) yield break;

        isBusy = true;
        isFollow = false;

        surtrSword.DOMove(new Vector3(0, 6, 0), 1f);
        surtrSword.DORotate(new Vector3(0, 0, -90f), 1f);

        yield return new WaitForSeconds(0.6f);


        GameObject[] flameList = new GameObject[6];
        for(int i = 0; i < 6; i++)
        {
            flameList[i] = Instantiate(effectPrefabs[4], new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-9.5f, 9.5f), 0), Quaternion.identity);
        }
        yield return new WaitForSeconds(2f);

        _Warner.RemoveAllHitArea();
        for (int i = 0; i < 6; i++)
        {
            Destroy(flameList[i]);
        }
        isFollow = true;
        isBusy = false;
    }

    // �Ҳ� ȸ��
    public IEnumerator Pattern3()
    {
        if (isBusy) yield break;
        isBusy = true;
        isFollow = false;

        Vector3 cachedPlayerPosition = player.transform.position;
        float originalRotation = Mathf.Atan2(cachedPlayerPosition.y - surtrSword.position.y, cachedPlayerPosition.x - surtrSword.position.x) * 180 / Mathf.PI;
        var rotateTween = surtrSword.DORotate(new Vector3(0, 0, originalRotation), 0.5f);
        yield return rotateTween.WaitForCompletion();
        Instantiate(effectPrefabs[6], surtrSword.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);

        var magmaPool = Instantiate(effectPrefabs[5], surtrSword.position + (cachedPlayerPosition - surtrSword.position).normalized * 2f, Quaternion.identity);
        MagmaPool magmaPoolScript = magmaPool.GetComponent<MagmaPool>();
        magmaPoolScript.SetDirection((cachedPlayerPosition - surtrSword.position).normalized);

        yield return new WaitForSeconds(3f);

        isBusy = false;
        isFollow = true;
        yield return null;
    }

    // �ۿ� ����
    public IEnumerator Pattern4()
    {
        if (isBusy) yield break;
        isBusy = true;
        isFollow = false;

        Vector3 cachedPlayerPosition = player.transform.position;
        float originalRotation = Mathf.Atan2(cachedPlayerPosition.y - surtrSword.position.y, cachedPlayerPosition.x - surtrSword.position.x) * 180 / Mathf.PI;
        var rotateTween = surtrSword.DORotate(new Vector3(0, 0, originalRotation), 0.5f);
        yield return rotateTween.WaitForCompletion();
        Instantiate(effectPrefabs[6], surtrSword.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);

        Instantiate(effectPrefabs[7], surtrSword.position, surtrSword.rotation);

        yield return new WaitForSeconds(1f);

        isBusy = false;
        isFollow = true;
        yield return null;
    }

    // ȭ�� ��ǳ
    public IEnumerator Pattern5()
    {
        if (isBusy) yield break;


        yield return null;
    }
}
