using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{

    public new string name;
    public int rank;//category 관련 등급
    int _cost;
    public int cost
    {
        get{return _cost;}
    }
    public CATEGORY category;
    public enum CATEGORY
    {
        KING, ARCHER
    }
    
    public Status stat;
    public int level;
    public int hp;
    int _maxHp;
    public int mp;
    int _maxMp;
    public int movePoint;
    public UnitAnimCtrl UnitAnimCtrl;
    public EquipItem equipItem;
    public List<int> skillList = new List<int>();
    List<Passive.PASSIVENUM> passiveList = new List<Passive.PASSIVENUM>();

    //전투시 유닛 위치
    [SerializeField] Map.Pos pos;
    //전투시 적대관계
    public IDENTIFY identify;
    [HideInInspector]
    public Unit attackUnit;
    public IDENTIFY Identify
    {
        get { return identify; }
    }
    public AttackRangeManager.ATTACKRANGE range;

    public enum IDENTIFY
    {
        ALLY, ENEMY
    }
    [Serializable]
    public struct Status
    {
        public int STR;
        public int DEX;
        public int CON;
        public int INT;
        public int WIS;
        public int LUK;
        public Status(int STR,int DEX,int CON,int INT,int WIS,int LUK)
        {
            this.STR = STR;
            this.DEX = DEX;
            this.CON = CON;
            this.INT = INT;
            this.WIS = WIS;
            this.LUK = LUK;
        }
        public Status(string STR, string DEX, string CON, string INT, string WIS, string LUK)
        {
            this.STR = int.Parse(STR);
            this.DEX = int.Parse(DEX);
            this.CON = int.Parse(CON);
            this.INT = int.Parse(INT);
            this.WIS = int.Parse(WIS);
            this.LUK = int.Parse(LUK);
        }
    }
    [Serializable]
    public struct EquipItem
    {
        Item weapon;
        Item armor;
        Item assi;

        public Item Weapon
        {
            set { weapon = value; }
            get { return weapon; }
        }
        public Item Armor
        {
            set { armor = value; }
            get { return armor; }
        }
        public Item Assi
        {
            set { assi = value; }
            get { return assi; }
        }
        /// <summary>
        /// HP=0,MP=1,ATK=2,DEF=3
        /// </summary>
        /// <param name="statNum"></param>
        /// <returns></returns>
        public int GetStat(int statNum)
        {
            int result = 0;
            if (weapon)
                result += weapon.GetStat(statNum);
            if (armor)
                result += armor.GetStat(statNum);
            if (assi)
                result += assi.GetStat(statNum);
            return  result;
        }
    }
    public Texture image;
    // Use this for initialization
    
    public void Init(string name, int level, int rank, CATEGORY category,int cost,Status stat)
    {
        this.name = name;
        this.level = level;
        this.rank = rank;
        this.category = category;
        this._cost = cost;
        this.stat = stat;
        movePoint = UnitCategoryManager.GetInfo(category,rank).movePoint;
        range = UnitCategoryManager.GetInfo(category, rank).range;
        try {
            for (int i = 0; i < UnitCategoryManager.GetInfo(category, rank).skillNum.Length; i++)
            {
                skillList.Add(UnitCategoryManager.GetInfo(category, rank).skillNum[i]);
            }
        }
        catch (NullReferenceException)
        {
            if(UnitCategoryManager.GetInfo(category, rank).skillNum == null)
            {
                
            }
            else
            {
                Debug.Log("확인 필요");
            }
        }
        
        hp = maxHp;
        mp = maxMp;
        pos = new Map.Pos(-1, -1);
    }
    public int MovePoint
    {
        get { return movePoint + 0; }//버프값이 적용된 값을 반환 }

    }

    public Map.Pos Pos
    {
        get { return pos; }
        set { pos = value; }
    }
    public void SetPos(int x, int y)
    {
        pos.Set(x, y);
    }
    public int atk
    {
        get { return (int)(stat.STR * UnitCategoryManager.GetCategory(category).GetInfo(rank).atkUp * 0.1f*level) + equipItem.GetStat(2); }
    }
    public int def
    {
        get { return (int)(stat.CON * UnitCategoryManager.GetCategory(category).GetInfo(rank).defUp * 0.1f * level) + equipItem.GetStat(3); }
    }
    public int maxHp
    {
        get { return UnitCategoryManager.GetCategory(category).GetInfo(rank).hp * level + equipItem.GetStat(0); }
    }
    public int maxMp
    {
        get { return UnitCategoryManager.GetCategory(category).GetInfo(rank).mp * level + equipItem.GetStat(1); }
    }

    public void Attack(Unit unit)
    {

        //명중 계산
        //데미지 계산
        unit.hp -= atk - unit.def;
    }
    public void Attack()
    {
        attackUnit.hp -= (int)Mathf.Clamp(atk - attackUnit.def,0f, attackUnit.hp);
        Debug.Log(atk - attackUnit.def);
        attackUnit = null;
    }
}
