using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;
using DG.Tweening;

public class Surtr : Boss
{
    public GameObject sword;
    public GameObject flame;
    public GameObject player;

    Vector3 playerPos;
    Vector3 velocity;

    public float swordSpeed = 10f;
    private float distance = 0f;
    private float stopTime = 0f;
    private float stopCoolTime = 0f;
    private bool isFollow = true;

    CSVReader bookDB;

    private void Start()
    {
        bookDB = BookManager.Instance.bookDB;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().localPosition;
    }

    private void Update()
    {
        //검 이동 코드. 따라오다가 가끔씩 stopTime만큼 멈춘다. (stopCoolTime초마다 30%확률로 정지 시도)

        if (!isFollow) return;

        playerPos = player.transform.position;

        distance = Vector3.Distance(playerPos, sword.transform.position);

        if(!(stopCoolTime<0f && Random.value<0.3f))
        {
            velocity = (playerPos - sword.transform.position).normalized * Mathf.Min(swordSpeed, distance*2/2);
        }
        else if (stopTime <= 0f)
        {
            velocity = Vector3.zero;
            stopTime = 1f;
            //StartCoroutine(Wait());
        }
        if (stopTime > 0f)
        {
            stopTime -= Time.deltaTime;
        }
        else
        {
            sword.transform.position = sword.transform.position + velocity * Time.deltaTime;
        }
        if(stopCoolTime<0f) stopCoolTime = 1f;
        stopCoolTime-=Time.deltaTime;


        //디버그
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
    }

    // 불의 참격
    public IEnumerator Pattern1()
    {
        if (isBusy) yield break;

        string skill = "Surtr1";
        int isCw = Random.Range(0, 2);
        int historyLevel = BookManager.Instance.CheckBookEquipped(skill);
        float effect1 = float.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(skill) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("effect1")]);

        float rot = Mathf.Atan2(playerPos.y - sword.transform.position.y, playerPos.x - sword.transform.position.x) * 180 / Mathf.PI - sword.transform.rotation.z + 30 + 120 * isCw;
        if (playerPos.x < sword.transform.position.x){
            rot += 180f;
            if (rot > 180f)
            {
                rot -= 360f;
            }
        }



        Debug.Log("Yay");

        yield return null;
    }
    // 마그마 기둥
    public IEnumerator Pattern2()
    {
        
        Debug.Log("Y");
        if (isBusy) yield break;

        isBusy = true;
        isFollow = false;
        sword.transform.DOMove(new Vector3(0, 6, 0), 1f);
        sword.transform.DORotate(new Vector3(0, 0, 0), 1f);

        yield return new WaitForSeconds(0.6f);


        GameObject[] flameList = new GameObject[6];
        for(int i = 0; i < 6; i++)
        {
            flameList[i]=Instantiate(flame, new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-9.5f, 9.5f), 0), Quaternion.identity);
        }
        yield return new WaitForSeconds(2f);

        RemoveAllHitArea();
        for (int i = 0; i < 6; i++)
        {
            Destroy(flameList[i]);
        }
        isFollow = true;
    }

    // 불꽃 회전
    public IEnumerator Pattern3()
    {
        if (isBusy) yield break;


        yield return null;
    }

    // 작열 광선
    public IEnumerator Pattern4()
    {
        if (isBusy) yield break;


        yield return null;
    }

    // 화염 폭풍
    public IEnumerator Pattern5()
    {
        if (isBusy) yield break;


        yield return null;
    }
}
