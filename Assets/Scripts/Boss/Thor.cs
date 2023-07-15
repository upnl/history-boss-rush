using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Thor : Boss
{
    public GameObject mjolnir;

    public float mjolnirSpeed = 3.5f;

    CSVReader bookDB;

    private void Start()
    {
        bookDB = BookManager.Instance.bookDB;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1 " + isBusy);
            StartCoroutine(Pattern1());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4 " + isBusy);
            StartCoroutine(Pattern4());
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
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().localPosition;

        // TODO 묠니르를 들어올리는 모션

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
        while (!mjolnir.GetComponent<PolygonCollider2D>().IsTouching(GameManager.Instance.FieldManager.wall1.GetComponent<BoxCollider2D>()) &&
            !mjolnir.GetComponent<PolygonCollider2D>().IsTouching(GameManager.Instance.FieldManager.wall2.GetComponent<BoxCollider2D>()) &&
            !mjolnir.GetComponent<PolygonCollider2D>().IsTouching(GameManager.Instance.FieldManager.wall3.GetComponent<BoxCollider2D>()) &&
            !mjolnir.GetComponent<PolygonCollider2D>().IsTouching(GameManager.Instance.FieldManager.wall4.GetComponent<BoxCollider2D>()))
        {
            yield return null;
            mjolnir.transform.localPosition = mjolnir.transform.localPosition + mjolnirSpeed * Time.deltaTime * velocity;
        }
        // TODO 벽에 안 닿으면 영원히 패턴이 종료되지 않는 버그에 빠질 것!

        Vector3 tempMjolnirPos = mjolnir.transform.localPosition;

        // 묠니르가 벽에 닿으면 외벽 근처에 있는 플레이어 사망
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
        while (!mjolnir.GetComponent<PolygonCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
        {
            yield return null;
            mjolnir.transform.localPosition = mjolnir.transform.localPosition + mjolnirSpeed * Time.deltaTime * velocity;
        }
        // 묠니르가 토르에 닿으면 패턴 종료
        // TODO 안 돌아오면 영원히 패턴이 종료되지 않는 버그에 빠질 것!

        mjolnir.transform.localPosition = transform.localPosition;

        isBusy = false;
    }

    // 피뢰침 방전
    public IEnumerator Pattern2()
    {
        yield return null;
    }

    // 정전기 폭발
    public IEnumerator Pattern3()
    {
        yield return null;
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

        yield return new WaitForSeconds(1.4f - effect1);

        InstantiateHitCircle(transform.localPosition, 7f);

        // effect1 시간 기다리기
        yield return new WaitForSeconds(effect1);

        // 생성한 HitCircleAreaWarning 모두 제거하기
        RemoveAllHitArea();
        // TODO 전기 이펙트 및 플레이어 공격

        yield return null;

        StartCoroutine(PassivePattern());
    }

    // 전자 갑옷
    public IEnumerator PassivePattern()
    {
        InstantiateHitCircle(transform.localPosition, 3f);
        yield return new WaitForSeconds(3f);
        RemoveAllHitArea();
        yield return null;
        isBusy = false;
    }
}
