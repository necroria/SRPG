using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {
    /*
    1)유닛 카테고리별 공격 방식 AI필요
    ex)전사류 캐릭이라면 바로이동하면서 공격하면 됨
    법사류면 마법 사용 가능 여부 체크, 못하면 공격이 가능한지 체크 공격이 가능하면 떄릴지를 체크(판단 기준 필요)
    원거리는 기본적으로 최대사거리에서 공격
    
    2)탐색 방식 -> 맵정보가 필요, 상대 유닛 배치를 알아야함
    이동에 관한 AI필요
    1:일정 거리안의 공격을 추적하면서 공격
    2:제자리 지키기
    3:특정범위안의 적만 공격?
    4:공격방식에 따라 이동방식이 바뀌어야함

    3)스테이지에 따른 AI
    승리조건에 따라 AI필요
    특정맵에서 쓰이는 AI

    4)카테고리에 따른 순서 및 조작
    법사가 무작정 돌격하면 망함
    탱커라면 앞에 있다던가 하는게 필요
    딜러나 강한 녀석이 때려서 죽일 수 있다면 죽이는게 우선 되어야함

    5)
     */
    public static void FootManAI(Unit unit)
    {

    }
}
