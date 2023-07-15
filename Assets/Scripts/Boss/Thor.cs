using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thor : Boss
{
    public IEnumerator Pattern1()
    {
        // 패턴 사용 1초 전에 코루틴 Pattern1() 호출

        Vector3 playerPos = new Vector3();
        // TODO 플레이어의 현재 위치 파악
        int historyLevel = BookManager.Instance.CheckBookEquipped("Thor1");

        if (true)   // TODO DB에서 Thor1의 effect1(미리 보여주는 시간)을 가져오기 -> 이 값에 따라 조건문 분기
        {
            // 얼만큼 기다려야 하는가 = 1초 - effect1
            yield return new WaitForSeconds(0.1f);
        }

        // HitBoxAreaWarning을 (망치 앞 -> playerPos)과 네 외벽 근처에 생성
        // effect1 시간 기다리기
        // 생성한 HitBoxAreaWarning 모두 제거하기
        // 묠니르(망치) 날리기 -> 피격 범위에 닿으면 플레이어 사망
        // 묠니르가 벽에 닿을 때까지 대기
        // 묠니르가 벽에 닿으면 외벽 근처에 있는 플레이어 사망
        // 시간 조금 기다리면서 다시 HitBoxAreaWarning을 (망치 앞 -> 토르)에 생성
        // effect1 시간 기다리기
        // 생성한 HitBoxAreaWarning 모두 제거하기
        // 묠니르(망치) 날리기 -> 피격 범위에 닿으면 플레이어 사망
        // 묠니르가 토르에 닿으면 패턴 종료
    }
}
