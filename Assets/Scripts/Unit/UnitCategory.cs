using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCategory {
    Unit.CATEGORY category;
    [Serializable]
    public struct Info
    {
        public string rankName;
        public float atkUp;
        public float defUp;
        public int hp;
        public int mp;
        public int[] passiveNum;
        public int[] skillNum;
        public int[] consumingMovePoint;
        public int movePoint;
        public AttackRangeManager.ATTACKRANGE range;
    }
    List<Info> infoList = null;
    public Info GetInfo(int rank)
    {
        return infoList[rank];
    }
    
    //랭크에 따른 전직명, 레벨업에 따른 스탯 상승 수치, 기본 패시브 종류, 기본 스킬 종류
    public void Set(Unit.CATEGORY category, List<Info> infoList)
    {
        this.category = category;
        this.infoList = infoList;
    }

}
