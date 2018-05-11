using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class Player : MonoBehaviour {
    int level;
    int gold;
    int lastStage;
    public int[] mainUnitList = new int[3];
    Transform tr;
    //public List<GameObject> unitPrefabList;
    public List<GameObject> unitObjectList = new List<GameObject>();
    public int unitCount;
    struct MyUnit
    {
        public Unit unit;
        public bool usable;

        public MyUnit(Unit unit, bool usable)
        {
            this.unit = unit;
            this.usable = usable;
        }
    }
    [Serializable]
    public class Deck
    {
        int[] slot;

        int slotCount = 5;
        public Deck(int slotCount)
        {
            this.slotCount = slotCount;
            slot = new int[slotCount];
            for(int i=0; i < slotCount; i++)
            {
                slot[i] = -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public int GetUnitNumber(int slotNumber)
        {
            return slot[slotNumber - 1];
        }
        public void SetUnitInSlot(int slotNumber, int unitNumber)
        {
            slot[slotNumber - 1] = unitNumber;
        }
    }

    Dictionary<int, Deck> deckDic = new Dictionary<int, Deck>();
    public Deck GetDeck(int battleNum)
    {
        return deckDic[battleNum];
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    List<MyUnit> myUnits = new List<MyUnit>();
	// Use this for initialization
	public void Init () {

        tr = GetComponent<Transform>();

        
        
        JsonData jsonData = null;
        do
        {
            Debug.Log("플레이어 데이터를 읽는 중입니다.");
            jsonData = FileUtil.LoadPlayerData();
        } while (jsonData==null);

        SetMyDeck(jsonData["decks"]);
        
        

        level = (int)jsonData["level"];
        gold = (int)jsonData["gold"];
        
        JsonData unitListJson = jsonData["main_unit_list"];
        mainUnitList[0] = (int)unitListJson[0];
        mainUnitList[1] = (int)unitListJson[1];
        mainUnitList[2] = (int)unitListJson[2];
        lastStage = (int)jsonData["last_stage"];

        unitObjectList = UnitManager.GetPlayerUnitList();
        unitCount = unitObjectList.Count;
        for (int i = 0; i < unitCount; i++)
        {
            unitObjectList[i].transform.parent = tr;
        }
        
        myUnits = GetMyUnitList(jsonData["units"]);
    }
    public Unit GetUnit(int num)
    {
        return myUnits[num].unit;
    }
    public bool GetUnitUsable(int num)
    {
        return myUnits[num].usable;
    }
    public GameObject GetUnitObject(int num)
    {
        return unitObjectList[num];
    }
    public void SetActiveAllUnit(bool value)
    {
        for(int i = 0; i < unitCount; i++)
        {
            unitObjectList[i].SetActive(value);
        }
    }
    //덱 정보 저장
    void SetMyDeck(JsonData jsonData)
    {
        SetDeck("1", jsonData);
    }
    void  SetDeck(string key,JsonData jsonData)
    {
        JsonData deckData = jsonData[key];
        Deck deck = new Deck(deckData.Count);
        for (int i = 0; i < deckData.Count; i++)
        {
            deck.SetUnitInSlot(i+1, (int)deckData[i]);
        }
        deckDic[int.Parse(key)] = deck;
    }
    //데이터를 읽어서 저장
    List<MyUnit> GetMyUnitList(JsonData jsonData)
    {        
        List<MyUnit> myUnits = new List<MyUnit>();
        
        for (int i = 0; i < jsonData.Count; i++)
        {
            string name = (string)jsonData[i][0]["name"];
            int level = (int)jsonData[i][0]["level"];
            int rank = (int)jsonData[i][0]["rank"];
            int category = (int)jsonData[i][0]["category"];
            int cost = (int)jsonData[i][0]["cost"];
            JsonData statData = jsonData[i][0]["stat"];
            Unit.Status status = new Unit.Status((int)statData[0], (int)statData[1],
                (int)statData[2], (int)statData[3], (int)statData[4], (int)statData[5]);

            Unit unit = unitObjectList[i].GetComponent<Unit>();
            unit.Init(name, level, rank, (Unit.CATEGORY)category, cost, status);
            myUnits.Add(new MyUnit(unit, (bool)jsonData[i]["usable"]));
        }
        return myUnits;
    }
}
