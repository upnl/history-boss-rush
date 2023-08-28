using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Thor : Boss
{
    [SerializeField] private Transform mjolnir;

    [SerializeField] private List<Sprite> thorSpriteList;
    [SerializeField] private SpriteRenderer ThorSpriteRenderer;

    [SerializeField] private float mjolnirSpeed = 3.5f;

    [SerializeField] private List<GameObject> effectPrefabs;

    CSVReader bookDB;

    private void Start()
    {
        if (effectPrefabs == null)
            Debug.LogError("EffectPrefabs is Null");

        bookDB = BookManager.Instance.bookDB;
        playerController = player.GetComponent<PlayerController>();
        pattern += UseAPattern;
        cooltime = Random.Range(1.5f, 3f);
        currentGauge = 0f;

        MoveToPosition(mjolnir, transform.position);

        ThorSpriteRenderer.sprite = thorSpriteList[0];

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
                cooltime = Random.Range(1.5f, 2f);
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
        Vector3 playerPos = player.transform.position;

        // TODO 묠니르를 들어올리는 모션
        ThorSpriteRenderer.sprite = thorSpriteList[1];
        MoveToPosition(mjolnir, transform.position + new Vector3(0f, 2f, 0f));

        // DB에서 Thor1의 effect1(미리 보여주는 시간)을 가져오기
        // 얼만큼 기다려야 하는가: 0.5초 - effect1
        yield return new WaitForSeconds(0.5f - effect1);

        // HitBoxAreaWarning을 (망치 앞 -> playerPos)과 네 외벽 근처에 생성
        _Warner.InstantiateHitBox(mjolnir.transform.position, playerPos, 1f);

        Vector3 v = playerPos - mjolnir.transform.position;
        LookAtDirection(mjolnir, new Vector2(v.x, v.y));

        // effect1 시간 기다리기
        yield return new WaitForSeconds(effect1);

        // 생성한 HitBoxAreaWarning 모두 제거하기
        _Warner.RemoveAllHitArea();

        yield return null;

        _Warner.InstantiateHitBoxInCenter(new Vector3(-9.5f, 0f, 0f), 0f);
        _Warner.InstantiateHitBoxInCenter(new Vector3(9.5f, 0f, 0f), 0f);
        _Warner.InstantiateHitBoxInCenter(new Vector3(0f, 9.5f, 0f), 90f);
        _Warner.InstantiateHitBoxInCenter(new Vector3(0f, -9.5f, 0f), 90f);

        // 묠니르(망치) 날리기 -> 피격 범위에 닿으면 플레이어 사망
        // 묠니르가 벽에 닿을 때까지 대기
        Vector3 velocity = (playerPos - transform.position - new Vector3(0f, 2f, 0f)).normalized;
        Collider2D mjolnirCollider = mjolnir.GetComponent<Collider2D>();
        while (!mjolnirCollider.IsTouching(GameManager.Instance.FieldManager.wall1.GetComponent<Collider2D>()) &&
            !mjolnirCollider.IsTouching(GameManager.Instance.FieldManager.wall2.GetComponent<Collider2D>()) &&
            !mjolnirCollider.IsTouching(GameManager.Instance.FieldManager.wall3.GetComponent<Collider2D>()) &&
            !mjolnirCollider.IsTouching(GameManager.Instance.FieldManager.wall4.GetComponent<Collider2D>()))
        {
            yield return null;
            mjolnir.transform.position = mjolnir.transform.position + mjolnirSpeed * Time.deltaTime * velocity;
            if (mjolnirCollider.IsTouching(playerCollider))
            {
                // AudioManager.Instance.PlaySfx(3);
                // playerBehaviour.GetDamaged();
            }
        }
        // TODO 벽에 안 닿으면 영원히 패턴이 종료되지 않는 버그에 빠질 것!

        Vector3 tempMjolnirPos = mjolnir.position;
        AudioManager.Instance.PlaySfx(3);

        // 묠니르가 벽에 닿으면 외벽 근처에 있는 플레이어 사망
        Instantiate(effectPrefabs[0], new Vector3(9.5f, -9.5f, 0f), Quaternion.Euler(0f, 0f, 90f));
        Instantiate(effectPrefabs[0], new Vector3(9.5f, -9.5f, 0f), Quaternion.Euler(0f, 0f, 180f));
        Instantiate(effectPrefabs[0], new Vector3(-9.5f, +9.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
        Instantiate(effectPrefabs[0], new Vector3(-9.5f, +9.5f, 0f), Quaternion.Euler(0f, 0f, 270f));

        //_Warner.AttackOnAllHitArea();

        yield return null;

        _Warner.RemoveAllHitArea();
        // TODO 전기 이펙트

        ThorSpriteRenderer.sprite = thorSpriteList[0];

        yield return null;

        // 시간 조금 기다리면서 다시 HitBoxAreaWarning을 (망치 앞 -> 토르)에 생성
        // 돌아오는 망치의 경로 미리 표시하기
        _Warner.InstantiateHitBox(tempMjolnirPos, transform.position, 1f, Vector3.Distance(tempMjolnirPos, transform.position));

        yield return new WaitForSeconds(0.3f);
        // effect1 시간 기다리기
        // 생성한 HitBoxAreaWarning 모두 제거하기

        _Warner.RemoveAllHitArea();

        yield return null;

        // 묠니르(망치) 날리기 -> 피격 범위에 닿으면 플레이어 사망
        velocity = (transform.position - tempMjolnirPos).normalized;
        while (!mjolnir.GetComponent<Collider2D>().IsTouching(this.GetComponent<Collider2D>()))
        {
            yield return null;
            mjolnir.position = mjolnir.transform.position + mjolnirSpeed * Time.deltaTime * velocity;
            if (mjolnir.GetComponent<Collider2D>().IsTouching(playerCollider))
            {
                // AudioManager.Instance.PlaySfx(3);
                // playerBehaviour.GetDamaged();
            }
        }
        // 묠니르가 토르에 닿으면 패턴 종료
        // TODO 안 돌아오면 영원히 패턴이 종료되지 않는 버그에 빠질 것!

        ThorSpriteRenderer.sprite = thorSpriteList[0];
        MoveToPosition(mjolnir, transform.position);

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

        ThorSpriteRenderer.sprite = thorSpriteList[1];
        MoveToPosition(mjolnir, transform.position + new Vector3(0f, 2f, 0f));

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

        yield return new WaitForSeconds(0.9f - effect1);


        _Warner.InstantiateHitBoxInCenter(stone1Pos, 0f);
        _Warner.InstantiateHitBoxInCenter(stone1Pos, 90f);
        _Warner.InstantiateHitBoxInCenter(stone2Pos, 0f);
        _Warner.InstantiateHitBoxInCenter(stone2Pos, 90f);
        _Warner.InstantiateHitBoxInCenter(stone3Pos, 0f);
        _Warner.InstantiateHitBoxInCenter(stone3Pos, 90f);
        _Warner.InstantiateHitBoxInCenter(new Vector3(-9.5f, 0f, 0f), 0f);
        _Warner.InstantiateHitBoxInCenter(new Vector3(9.5f, 0f, 0f), 0f);
        _Warner.InstantiateHitBoxInCenter(new Vector3(0f, 9.5f, 0f), 90f);
        _Warner.InstantiateHitBoxInCenter(new Vector3(0f, -9.5f, 0f), 90f);

        GameObject stone1 = Instantiate(effectPrefabs[2], stone1Pos, Quaternion.identity, transform);
        GameObject stone2 = Instantiate(effectPrefabs[2], stone2Pos, Quaternion.identity, transform);
        GameObject stone3 = Instantiate(effectPrefabs[2], stone3Pos, Quaternion.identity, transform);

        yield return new WaitForSeconds(effect1 - 0.5f);

        // TODO 전기 충전되는 이펙트
        Instantiate(effectPrefabs[3], stone1Pos, Quaternion.identity, transform);
        Instantiate(effectPrefabs[3], stone2Pos, Quaternion.identity, transform);
        Instantiate(effectPrefabs[3], stone3Pos, Quaternion.identity, transform);

        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySfx(3);

        // AttackOnAllHitArea();
        Instantiate(effectPrefabs[0], stone1Pos, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(effectPrefabs[0], stone1Pos, Quaternion.Euler(0f, 0f, 90f));
        Instantiate(effectPrefabs[0], stone1Pos, Quaternion.Euler(0f, 0f, 180f));
        Instantiate(effectPrefabs[0], stone1Pos, Quaternion.Euler(0f, 0f, 270f));
        Instantiate(effectPrefabs[0], stone2Pos, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(effectPrefabs[0], stone2Pos, Quaternion.Euler(0f, 0f, 90f));
        Instantiate(effectPrefabs[0], stone2Pos, Quaternion.Euler(0f, 0f, 180f));
        Instantiate(effectPrefabs[0], stone2Pos, Quaternion.Euler(0f, 0f, 270f));
        Instantiate(effectPrefabs[0], stone3Pos, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(effectPrefabs[0], stone3Pos, Quaternion.Euler(0f, 0f, 90f));
        Instantiate(effectPrefabs[0], stone3Pos, Quaternion.Euler(0f, 0f, 180f));
        Instantiate(effectPrefabs[0], stone3Pos, Quaternion.Euler(0f, 0f, 270f));

        yield return null;

        _Warner.RemoveAllHitArea();
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

        ThorSpriteRenderer.sprite = thorSpriteList[0];
        MoveToPosition(mjolnir, transform.position);

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
        Vector3 playerPos = player.GetComponent<Transform>().position;

        // TODO 망치 올리기
        ThorSpriteRenderer.sprite = thorSpriteList[1];
        MoveToPosition(mjolnir, transform.position + new Vector3(0f, 2f, 0f));

        yield return new WaitForSeconds(0.8f - effect1);

        _Warner.InstantiateHitFan60(transform.position, playerPos, 28.4f);

        yield return new WaitForSeconds(effect1 - 0.2f);

        // TODO 망치 내리기
        ThorSpriteRenderer.sprite = thorSpriteList[0];
        MoveToPosition(mjolnir, transform.position);

        yield return new WaitForSeconds(0.2f);

        // TODO 전기 이펙트 및 플레이어 공격
        AudioManager.Instance.PlaySfx(3);
        Vector2 direction = playerPos - transform.position;
        Instantiate(effectPrefabs[4], transform.position, Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) / Mathf.PI * 180f));

        yield return null;

        // 생성한 HitFan60AreaWarning 모두 제거하기
        _Warner.RemoveAllHitArea();

        yield return null;

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
        ThorSpriteRenderer.sprite = thorSpriteList[1];
        MoveToPosition(mjolnir, transform.position + new Vector3(0f, 2f, 0f));

        yield return new WaitForSeconds(1.4f - effect1);

        _Warner.InstantiateHitCircle(transform.position, 5.5f);

        // effect1 시간 기다리기
        yield return new WaitForSeconds(effect1);

        // TODO 전기 이펙트 및 플레이어 공격
        AudioManager.Instance.PlaySfx(3);
        Instantiate(effectPrefabs[5], transform);

        yield return null;

        // 생성한 HitCircleAreaWarning 모두 제거하기
        _Warner.RemoveAllHitArea();

        yield return null;

        ThorSpriteRenderer.sprite = thorSpriteList[0];
        MoveToPosition(mjolnir, transform.position);

        // TODO 주인공이 사망하지 않았다면 퀘스트 누적
        GameManager.Instance.QuestManager.UpPatternSeeCount(3);

        StartCoroutine(PassivePattern());
    }

    // 전자 갑옷
    private bool isPassiveOn = false;
    public IEnumerator PassivePattern()
    {
        isPassiveOn = true;
        // GameObject hit = _Warner.InstantiateHitCircle(transform.position, 3f, true);
        //Debug.Log("PassivePattern " + hit.name);

        float time = Time.time + 3f;
        // TODO 이 3초 동안 계속 데미지 가함

        GameObject electricShield = Instantiate(effectPrefabs[1], transform);
        
        yield return new WaitForSeconds(3f);
        // _Warner.RemoveAllHitArea();

        Destroy(electricShield);
        isPassiveOn = false;
        isBusy = false;
    }

    public override void GetDamaged()
    {
        // 플레이어의 공격 반사
        if (isPassiveOn)
        {
            playerController.GetDamaged();
        }
            

        base.GetDamaged();
    }

    #region Utils

    public void MoveToPosition(Transform moveObject, Vector2 targetPosition)
    {
        moveObject.position = targetPosition;
        moveObject.rotation = Quaternion.identity;
    }

    public void LookAtDirection(Transform moveObject, Vector2 lookDirection)
    {
        moveObject.rotation = Quaternion.Euler(0f, 0f, CalculateDegreeWithVector(lookDirection));
    }

    private float CalculateDegreeWithVector(Vector2 lookDirection)
        => 270f + Mathf.Atan2(lookDirection.y, lookDirection.x) / Mathf.PI * 180f;

    #endregion
}
