using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using r = UnityEngine.Random;


public class Map : MonoBehaviour {
    public Dictionary<Pos, PosInfo> location = new Dictionary<Pos, PosInfo>();
    public List<PosInfo> posInfos;
    public Transform mapStartTr;
    public List<GameObject> unitPrefabs;
    //public BoxCollider coll;
    public int moveSpeed;//게임에서 오브젝트 이동속도
    // Use this for initialization
    public int mapXSize;
    public int mapYSize;
    public List<GameObject> unitAllyList = new List<GameObject>();
    public List<GameObject> unitEnemyList = new List<GameObject>();
    public int maxTurn;
    [Serializable]
    public class PosInfo
    {
        [SerializeField]int x;
        [SerializeField] int y;
        [SerializeField] POSTYPE type;
        [SerializeField] GameObject unit;
        public PosInfo(int x,int y,POSTYPE type=POSTYPE.NONE,GameObject unit=null)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            this.unit = unit;
            if (unit != null)
            {
                unit.GetComponent<Unit>().SetPos(x,y);
            }
        }
        public POSTYPE Type{
            get  { return type;}
        }
        public void SetUnit(GameObject unit=null)
        {
            if (unit == null)
            {
                this.unit = null;
                return;
            }
            if (unit.tag == "UNIT")
            {
                
                this.unit = unit;
                Unit unitScript = this.unit.GetComponent<Unit>();
                
                unitScript.SetPos(x, y);
                
            }
        }
        public GameObject GetUnit()
        {           
            return unit;
        }
        public override string ToString()
        {
            return "(" + x + "," + y + ") " + type + " " + unit;
        }
    }
    [Serializable]
    public struct Pos
    {
        public int x;
        public int y;
        
        public Pos(int x = -1, int y = -1)
        {
            this.x = x;
            this.y = y;
        }
        public void Set(int x = -1, int y = -1)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }
        public override bool Equals(System.Object obj)
        {
            Pos pos = (Pos)obj;
            return x == pos.x && y == pos.y;
        }
        public override int GetHashCode()
        {

            return x ^ y;
        }
        public static int GetLength(Pos a,Pos b)
        {
            return Mathf.Abs(a.x - b.x)+ Mathf.Abs(a.y - b.y);
        }

        public static Pos operator +(Pos a, Pos b)
        {
            return new Pos(a.x+b.x,a.y+b.y);
        }
        public static Pos operator -(Pos a, Pos b)
        {
            return new Pos(a.x - b.x, a.y - b.y);
        }
        public static bool ValidInBattle(Pos pos)
        {
            return pos.x >= 0 && pos.y >= 0;
        }
    }
    public enum POSTYPE{NONE, PLAIN,   }	

    public PosInfo GetPosInfo(int x,int y)
    {        
        return location[new Pos(x,y)];
    }
    public GameObject GetPosUnit(int x, int y)
    {
        return location[new Pos(x, y)].GetUnit();
    }
    public GameObject GetPosUnit(Pos pos)
    {
        return location[pos].GetUnit();
    }
    public void SetPosInfoUnit(GameObject unit,int x, int y)
    {
        Pos pos = new Pos(x, y);
        SetPosInfoUnit(unit, pos);    
    }
    public void SetPosInfoUnit(GameObject unit, Pos pos)
    {
        Unit unitScript = unit.GetComponent<Unit>();
        
        if (CheckVaildPos(unitScript.Pos))
        {
            location[unitScript.Pos].SetUnit();
        }

        location[pos].SetUnit(unit);
        
    }
    public POSTYPE GetPosType(int x, int y)
    {
        return location[new Pos(x, y)].Type;
    }
    public POSTYPE GetPosType(Pos pos)
    {
        return location[pos].Type;
    }

    public void Init(JsonData data)
    {

        //data는 나중에 전체적인 유닛위치, 지형 정보같은 데이터들
        mapXSize = (int)data["xSize"];
        mapYSize = (int)data["ySize"];
        maxTurn = (int)data["maxTurn"];
        //맵정보를 읽어서 위치마다 정보를 주는 함수
        //coll.size = Vector3.right * mapXSize  + Vector3.forward * mapYSize+ Vector3.up * 0.001f;
        transform.localScale = Vector3.forward * mapXSize / 10 + Vector3.right * mapYSize / 10 + Vector3.up;
        for (int y = 0; y < mapYSize; y++)
        {
            for (int x = 0; x < mapXSize; x++)
            {
                location[new Pos(x, y)] = new PosInfo(x, y, (POSTYPE)(int)data["tiles"][y*20+x]);
            }
        }
        SetStartingUnit(data["units"]);
        posInfos = new List<PosInfo>(location.Values);
    }
    public void SetStartingUnit(JsonData unitData)
    {
        Player player = MainManager.Instance.player;
        JsonData playerData = unitData["player"];
        JsonData playalbeUnitData = unitData["playableUnit"];
        JsonData enemyData = unitData["enemy"];
        
        Player.Deck deck = player.GetDeck(MainManager.Instance.battleType);

        for (int i=0;i< playerData.Count; i++)
        {
            int unitNumber = deck.GetUnitNumber(i+1);

            if (unitNumber == -1)
                continue;
            GameObject unit = player.unitObjectList[unitNumber];
            unit.SetActive(true);
            MoveUnit(unit.GetComponent<Unit>(), (int)playerData[i]["x"], (int)playerData[i]["y"]);
            unitAllyList.Add(player.unitObjectList[unitNumber]);
            Debug.Log(unitAllyList.Count);
        }
        for (int i = 0; i < playalbeUnitData.Count; i++)
        {
            GameObject obj = null;
            string str = (string)playalbeUnitData[i]["key"];
            if ((int)playalbeUnitData[i]["unit_type"] == 0)
            {

                obj = UnitManager.GetUnit(int.Parse(str));
            }
            else
            {
                obj = UnitManager.GetUnit((int)playalbeUnitData[i]["unit_type"],str);
            }
            SetUnit(obj.GetComponent<Unit>(), playalbeUnitData[i], 0);
            unitAllyList.Add(obj);
        }
        for (int i = 0;i< enemyData.Count; i++)
        {
            GameObject obj = null;
            string str = (string)enemyData[i]["key"];
            if ((int)enemyData[i]["unit_type"] == 0)
            {

                obj = UnitManager.GetUnit(int.Parse(str));
            }
            else
            {
                obj = UnitManager.GetUnit((int)enemyData[i]["unit_type"], str);
            }
            SetUnit(obj.GetComponent<Unit>(), enemyData[i], 1);
            unitEnemyList.Add(obj);
        }        
    }
    public void SetUnit(Unit unit,JsonData data,int identity)
    {
        
        JsonData statData = data["stat"];
        Unit.Status stat;
        if (statData.IsString)
        {
            stat = UnitManager.GetStat((string)statData);
        }else
        {
            stat = new Unit.Status((int)statData[0], (int)statData[1],
                (int)statData[2], (int)statData[3], (int)statData[4], (int)statData[5]);
        }
        
        unit.Init((string)data["name"], (int)data["level"], (int)data["rank"], (Unit.CATEGORY)(int)data["category"], -1,stat);
        unit.identify = (Unit.IDENTIFY)identity;
        MoveUnit(unit, (int)data["x"], (int)data["y"]);
    }
    public void MoveUnit(Unit unit,int x, int y, bool posInfoChange = true)
    {
        Pos pos = new Pos(x, y);
        MoveUnit(unit, pos, posInfoChange);
    }
    public void MoveUnit(Unit unit, Pos pos,bool posInfoChange=true)
    {
        GameObject unitObject = unit.gameObject;
        unitObject.transform.position = PosToWorldPos(pos);
        if(posInfoChange)
            SetPosInfoUnit(unitObject, pos);        
    }
    
    public Pos WorldPosToMapPos(Vector3 point)
    {
        Vector3 mapPoint = point - mapStartTr.position;
        return new Pos(-(int)mapPoint.z, (int)mapPoint.x);
    }

    public Vector3 PosToWorldPos(int x,int y)
    {
        return new Vector3(0.5f + y, 0, -0.5f - x) + mapStartTr.position;
    }
    public Vector3 PosToWorldPos(Pos pos)
    {
        return new Vector3(0.5f + pos.y, 0, -0.5f - pos.x) + mapStartTr.position;
    }
    public bool CheckVaildPos(Pos pos)
    {
        if (pos.x > mapXSize - 1)
        {
            return false;
        }
        //pos.x-1
        if (pos.x < 0)
        {
            return false;
        }
        //pos.y+1
        if (pos.y > mapYSize - 1)
        {
            return false;
        }
        //pos.y-1
        if (pos.y < 0)
        {
            return false;
        }
        return true;
    }
}
