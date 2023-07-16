using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Thor : Boss
{
    public GameObject mjolnir;
    public GameObject stonePrefab;
    public Sprite thor1Sprite;
    public Sprite thor2Sprite;

    public float mjolnirSpeed = 3.5f;

    CSVReader bookDB;

    private void Start()
    {
        bookDB = BookManager.Instance.bookDB;
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        playerCollider = player.GetComponentInChildren<Collider2D>();
        pattern += UseAPattern;
        cooltime = Random.Range(1.5f, 3f);
        currentGauge = 0f;
        mjolnir.transform.localPosition = transform.localPosition;
        GetComponent<SpriteRenderer>().sprite = thor1Sprite;
        isBusy = false;
    }

    private void Update()
    {
        /*
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
        */

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
        int i = Random.Range(0, 4);
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
        }
    }

    // 천둥의 귀환
    public IEnumerator Pattern1()
    {
        if (isBusy) yield break;

        isBusy = true;
        // 패턴 사용 0.5초 전에 코루틴 Pattern1() 호출

        string skill = "Thor1";

        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        // 플레이어의 현재 위치 확인
        Vector3 playerPos = player.GetComponent<Transform>().localPosition;

        // TODO 묠니르를 들어올리는 모션
        GetComponent<SpriteRenderer>().sprite = thor2Sprite;

        // DB에서 Thor1의 effect1(미리 보여주는 시간)을 가져오기
        // 얼만큼 기다려야 하는가: 0.5초 - effect1
        yield return new WaitForSeconds(0.5f - effect1);

        mjolnir.transform.localPosition = transform.localPosition;

        // HitBoxAreaWarning을 (망치 앞 -> playerPos)과 네 외벽 근처에 생성
        InstantiateHitBox(mjolnir.transform.localPosition, playerPos, 1f);

        Vector3 v = playerPos - mjolnir.transform.localPosition;
        mjolnir.transform.localRotation = Quaternion.Euler(0f, 0f, 270f + Mathf.Atan2(v.y, v.x) / Mathf.PI * 180f);

        // effect1 시간 기다리기
        yield return new WaitForSeconds(effect1);

        // 생성한 HitBoxAreaWarning 모두 제거하기
        RemoveAllHitArea();

        yield return null;

        InstantiateHitBoxInCenter(new Vector3(-9.5f, 0f, 0f), 0f);
        InstantiateHitBoxInCenter(new Vector3(9.5f, 0f, 0f), 0f);
        InstantiateHitBoxInCenter(new Vector3(0f, 9.5f, 0f), 90f);
        InstantiateHitBoxInCenter(new Vector3(0f, -9.5f, 0f), 90f);

        // 묠니르(망치) 날리기 -> 피격 범위에 닿으면 플레이어 사망
        // 묠니르가 벽에 닿을 때까지 대기
        Vector3 velocity = (playerPos - transform.localPosition).normalized;
        while (!mjolnir.GetComponent<Collider2D>().IsTouching(GameManager.Instance.FieldManager.wall1.GetComponent<Collider2D>()) &&
            !mjolnir.GetComponent<Collider2D>().IsTouching(GameManager.Instance.FieldManager.wall2.GetComponent<Collider2D>()) &&
            !mjolnir.GetComponent<Collider2D>().IsTouching(GameManager.Instance.FieldManager.wall3.GetComponent<Collider2D>()) &&
            !mjolnir.GetComponent<Collider2D>().IsTouching(GameManager.Instance.FieldManager.wall4.GetComponent<Collider2D>()))
        {
            yield return null;
            mjolnir.transform.localPosition = mjolnir.transform.localPosition + mjolnirSpeed * Time.deltaTime * velocity;
            if (mjolnir.GetComponent<Collider2D>().IsTouching(playerCollider))
            {
                AudioManager.Instance.PlaySfx(3);
                playerBehaviour.GetDamaged();
            }
        }
        // TODO 벽에 안 닿으면 영원히 패턴이 종료되지 않는 버그에 빠질 것!

        Vector3 tempMjolnirPos = mjolnir.transform.localPosition;
        AudioManager.Instance.PlaySfx(3);

        // 묠니르가 벽에 닿으면 외벽 근처에 있는 플레이어 사망
        AttackOnAllHitArea();

        yield return null;

        RemoveAllHitArea();
        // TODO 전기 이펙트 및 플레이어 공격

        yield return null;

        // 시간 조금 기다리면서 다시 HitBoxAreaWarning을 (망치 앞 -> 토르)에 생성
        // 돌아오는 망치의 경로 미리 표시하기
        InstantiateHitBox(tempMjolnirPos, transform.position, 1f, Vector3.Distance(tempMjolnirPos, transform.position));

        yield return new WaitForSeconds(0.3f);
        // effect1 시간 기다리기
        // 생성한 HitBoxAreaWarning 모두 제거하기

        RemoveAllHitArea();

        yield return null;

        // 묠니르(망치) 날리기 -> 피격 범위에 닿으면 플레이어 사망
        velocity = (transform.localPosition - tempMjolnirPos).normalized;
        while (!mjolnir.GetComponent<Collider2D>().IsTouching(this.GetComponent<Collider2D>()))
        {
            yield return null;
            mjolnir.transform.localPosition = mjolnir.transform.localPosition + mjolnirSpeed * Time.deltaTime * velocity;
            if (mjolnir.GetComponent<Collider2D>().IsTouching(playerCollider))
            {
                AudioManager.Instance.PlaySfx(3);
                playerBehaviour.GetDamaged();
            }
        }
        // 묠니르가 토르에 닿으면 패턴 종료
        // TODO 안 돌아오면 영원히 패턴이 종료되지 않는 버그에 빠질 것!

        mjolnir.transform.localPosition = transform.localPosition;

        GetComponent<SpriteRenderer>().sprite = thor1Sprite;

        // TODO 주인공이 사망하지 않았다면 퀘스트 누적
        GameManager.Instance.QuestManager.UpPatternSeeCount(0);

        isBusy = false;
    }

    // 피뢰침 방전
    public IEnumerator Pattern2()
    {
        if (isBusy) yield break;

        isBusy = true;
        // TODO 플레이어가 근접한 경우에 호출

        string skill = "Thor2";

        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        GetComponent<SpriteRenderer>().sprite = thor2Sprite;

        Vector3 stone1Pos = new Vector3(), stone2Pos = new Vector3(), stone3Pos = new Vector3();
        int r = UnityEngine.Random.Range(0, 4);
        switch (r)
        {
            case 0:
                stone1Pos = new Vector3(-6f, -4f, 0f);
                stone2Pos = new Vector3(0f, 6f, 0f);
                stone3Pos = new Vector3(3f, 0f, 0f);
                break;
            case 1:
                stone1Pos = new Vector3(3f, -6f, 0f);
                stone2Pos = new Vector3(0f, 4f, 0f);
                stone3Pos = new Vector3(-4f, 0f, 0f);
                break;
            case 2:
                stone1Pos = new Vector3(5f, 5f, 0f);
                stone2Pos = new Vector3(0f, -5f, 0f);
                stone3Pos = new Vector3(-5f, 0f, 0f);
                break;
            case 3:
                stone1Pos = new Vector3(-3f, 3f, 0f);
                stone2Pos = new Vector3(0f, -3f, 0f);
                stone3Pos = new Vector3(4f, 0f, 0f);
                break;
        }

        yield return new WaitForSeconds(0.7f - effect1);


        InstantiateHitBoxInCenter(stone1Pos, 0f);
        InstantiateHitBoxInCenter(stone1Pos, 90f);
        InstantiateHitBoxInCenter(stone2Pos, 0f);
        InstantiateHitBoxInCenter(stone2Pos, 90f);
        InstantiateHitBoxInCenter(stone3Pos, 0f);
        InstantiateHitBoxInCenter(stone3Pos, 90f);
        InstantiateHitBoxInCenter(new Vector3(-9.5f, 0f, 0f), 0f);
        InstantiateHitBoxInCenter(new Vector3(9.5f, 0f, 0f), 0f);
        InstantiateHitBoxInCenter(new Vector3(0f, 9.5f, 0f), 90f);
        InstantiateHitBoxInCenter(new Vector3(0f, -9.5f, 0f), 90f);

        GameObject stone1, stone2, stone3;
        stone1 = Instantiate(stonePrefab, stone1Pos, Quaternion.identity);
        stone2 = Instantiate(stonePrefab, stone2Pos, Quaternion.identity);
        stone3 = Instantiate(stonePrefab, stone3Pos, Quaternion.identity);

        yield return new WaitForSeconds(effect1 - 0.3f);

        // TODO 전기 충전되는 이펙트

        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.PlaySfx(3);

        AttackOnAllHitArea();

        yield return null;

        RemoveAllHitArea();
        // TODO 공격 판정

        for (int i = 25; i >= 1; i--)
        {
            stone1.GetComponent<SpriteRenderer>().color = new Color(stone1.GetComponent<SpriteRenderer>().color.r,
                stone1.GetComponent<SpriteRenderer>().color.g, stone1.GetComponent<SpriteRenderer>().color.b, i / 25f);
            stone2.GetComponent<SpriteRenderer>().color = new Color(stone2.GetComponent<SpriteRenderer>().color.r,
                stone2.GetComponent<SpriteRenderer>().color.g, stone2.GetComponent<SpriteRenderer>().color.b, i / 25f);
            stone3.GetComponent<SpriteRenderer>().color = new Color(stone3.GetComponent<SpriteRenderer>().color.r,
                stone3.GetComponent<SpriteRenderer>().color.g, stone3.GetComponent<SpriteRenderer>().color.b, i / 25f);
            yield return null;
        }

        Destroy(stone1);
        Destroy(stone2);
        Destroy(stone3);

        yield return null;

        GetComponent<SpriteRenderer>().sprite = thor2Sprite;

        // TODO 주인공이 사망하지 않았다면 퀘스트 누적
        GameManager.Instance.QuestManager.UpPatternSeeCount(1);

        isBusy = false;
    }

    // 정전기 폭발
    public IEnumerator Pattern3()
    {
        if (isBusy) yield break;

        isBusy = true;
        // TODO 플레이어가 근접한 경우에 호출

        string skill = "Thor3";

        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        // 플레이어의 현재 위치 확인
        Vector3 playerPos = player.GetComponent<Transform>().localPosition;

        // TODO 망치 올리기
        GetComponent<SpriteRenderer>().sprite = thor1Sprite;

        yield return new WaitForSeconds(1f - effect1);

        InstantiateHitFan60(transform.localPosition, playerPos, 28.4f);

        yield return new WaitForSeconds(effect1 - 0.2f);

        // TODO 망치 내리기

        yield return new WaitForSeconds(0.2f);
        
        AudioManager.Instance.PlaySfx(3);
        // TODO 전기 이펙트 및 플레이어 공격
        AttackOnAllHitArea();

        yield return null;

        // 생성한 HitFan60AreaWarning 모두 제거하기
        RemoveAllHitArea();

        yield return null;
        GetComponent<SpriteRenderer>().sprite = thor2Sprite;

        // TODO 주인공이 사망하지 않았다면 퀘스트 누적
        GameManager.Instance.QuestManager.UpPatternSeeCount(2);

        StartCoroutine(PassivePattern());
    }

    // 번개 흡수
    public IEnumerator Pattern4()
    {
        if (isBusy) yield break;

        isBusy = true;
        // TODO 플레이어가 근접한 경우에 호출

        string skill = "Thor4";

        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);


        // TODO 묠니르를 들어올리는 모션
        GetComponent<SpriteRenderer>().sprite = thor2Sprite;

        yield return new WaitForSeconds(1.4f - effect1);

        InstantiateHitCircle(transform.localPosition, 7f);

        // effect1 시간 기다리기
        yield return new WaitForSeconds(effect1);

        // TODO 전기 이펙트 및 플레이어 공격
        AudioManager.Instance.PlaySfx(3);
        AttackOnAllHitArea();

        yield return null;

        // 생성한 HitCircleAreaWarning 모두 제거하기
        RemoveAllHitArea();

        yield return null;
        GetComponent<SpriteRenderer>().sprite = thor2Sprite;

        // TODO 주인공이 사망하지 않았다면 퀘스트 누적
        GameManager.Instance.QuestManager.UpPatternSeeCount(3);

        StartCoroutine(PassivePattern());
    }

    // 전자 갑옷
    public IEnumerator PassivePattern()
    {
        GameObject hit = InstantiateHitCircle(transform.localPosition, 3f, true);
        //Debug.Log("PassivePattern " + hit.name);

        float time = Time.time + 3f;

        // TODO 이 3초 동안 계속 데미지 가함
        // TODO 플레이어의 공격 반사
        while (Time.time < time)
        {
            yield return null;
            if (hit.GetComponent<Collider2D>().IsTouching(playerCollider)) {
                playerBehaviour.GetDamaged();
            }
        }
        RemoveAllHitArea();
        yield return null;
        isBusy = false;
    }
}
