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
    public List<GameObject> unitPrefabList;
    List<GameObject> unitObjectList = new List<GameObject>();
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
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    List<MyUnit> myUnits = new List<MyUnit>();
	// Use this for initialization
	public void Init () {
        
        JsonData jsonData = null;
        do
        {
            Debug.Log("플레이어 데이터를 읽는 중입니다.");
            jsonData = FileUtil.LoadPlayerData();
        } while (jsonData==null);
        
        level = int.Parse(jsonData["level"].ToString());
        gold = int.Parse(jsonData["gold"].ToString());
        
        JsonData unitListJson = jsonData["main_unit_list"];
        mainUnitList[0] = int.Parse(unitListJson[0].ToString());
        mainUnitList[1] = int.Parse(unitListJson[1].ToString());
        mainUnitList[2] = int.Parse(unitListJson[2].ToString());
        lastStage = int.Parse(jsonData["last_stage"].ToString());

        for(int i = 0; i < unitPrefabList.Count; i++)
        {
            unitObjectList.Add(Instantiate<GameObject>(unitPrefabList[i], transform));
        }
        myUnits = GetMyUnitList(jsonData["units"]);
    }

    public GameObject GetUnitObject(int num)
    {
        return unitObjectList[num];
    }

    //데이터를 읽어서 저장
    List<MyUnit> GetMyUnitList(JsonData jsonData)
    {        
        List<MyUnit> myUnits = new List<MyUnit>();
        
        for (int i = 0; i < jsonData.Count; i++)
        {
            string name = jsonData[i][0]["name"].ToString();
            int level = int.Parse(jsonData[i][0]["level"].ToString());
            int rank = int.Parse(jsonData[i][0]["rank"].ToString());
            int category = int.Parse(jsonData[i][0]["category"].ToString());
            int cost = int.Parse(jsonData[i][0]["cost"].ToString());
            JsonData statData = jsonData[i][0]["stat"];
            Unit.Status status = new Unit.Status(statData[0].ToString(), statData[1].ToString(), 
                statData[2].ToString(), statData[3].ToString(), statData[4].ToString(), statData[5].ToString());

            Unit unit = unitObjectList[i].GetComponent<Unit>();
            unit.Init(name, level, rank, (Unit.CATEGORY)category, cost, status);
            myUnits.Add(new MyUnit(unit, bool.Parse(jsonData[i]["usable"].ToString())));
        }
        return myUnits;
    }
}
