using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using r = UnityEngine.Random;


public class Map : MonoBehaviour {
    public Dictionary<Pos, PosInfo> location = new Dictionary<Pos, PosInfo>();
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
    
    public class PosInfo
    {
        int x;
        int y;
        POSTYPE type;
        GameObject unit;
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
                this.unit.GetComponent<Unit>().SetPos(x, y);
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
        location[new Pos(x, y)].SetUnit(unit);       
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
        mapXSize = int.Parse( data["xSize"].ToString());
        mapYSize = int.Parse(data["ySize"].ToString());
        maxTurn = int.Parse(data["maxTurn"].ToString());
        //맵정보를 읽어서 위치마다 정보를 주는 함수
        //coll.size = Vector3.right * mapXSize  + Vector3.forward * mapYSize+ Vector3.up * 0.001f;
        transform.localScale = Vector3.forward * mapXSize / 10 + Vector3.right * mapYSize / 10 + Vector3.up;
        for (int y = 0; y < mapYSize; y++)
        {
            for (int x = 0; x < mapXSize; x++)
            {
                location[new Pos(x, y)] = new PosInfo(x, y, (POSTYPE)int.Parse(data["tiles"][y*20+x].ToString()));
            }
        }
        SetStartingUnit(data["units"]);
    }
    public void SetStartingUnit(JsonData unitData)
    {
        for(int i = 0;i< unitData.Count; i++)
        {
            if((Unit.IDENTIFY)int.Parse(unitData[i][3].ToString())==Unit.IDENTIFY.ALLY)
                unitAllyList.Add(CreateUnit(unitData[i]));
            else if((Unit.IDENTIFY)int.Parse(unitData[i][3].ToString()) == Unit.IDENTIFY.ENEMY)
                unitEnemyList.Add(CreateUnit(unitData[i]));
        }
        //unitAllyList.Add(CreateUnit(0, 0,0, Unit.IDENTIFY.ALLY, Unit.CATEGORY.ARCHER,level:50));
        //unitAllyList.Add(CreateUnit(9, 9,0,rank:1, level: 50));        
        //unitAllyList.Add(CreateUnit(19, 0,0, Unit.IDENTIFY.ALLY, Unit.CATEGORY.ARCHER,rank:1, level: 50));
        //unitEnemyList.Add(CreateUnit(19, 19,1, Unit.IDENTIFY.ENEMY,rank:1, level: 50));
        //unitEnemyList.Add(CreateUnit(0, 19, 1,Unit.IDENTIFY.ENEMY, level: 50));
        //unitEnemyList.Add(CreateUnit(10, 9,1,Unit.IDENTIFY.ENEMY, level: 50));
    }
    public GameObject CreateUnit(int x, int y,int unitIndex,Unit.IDENTIFY identify=Unit.IDENTIFY.ALLY,Unit.CATEGORY category = Unit.CATEGORY.KING,int rank = 0, int level = 1)
    {
        GameObject unitObject = Instantiate(unitPrefabs[unitIndex]);
        unitObject.name = String.Format("unit{0:D2}{1:D2}", x,y);
        SetPosInfoUnit(unitObject, x, y);
        Unit unit = unitObject.GetComponent<Unit>();
        
        unit.Init(unitObject.name,level,rank,category,3,new Unit.Status(r.Range(30,90), r.Range(30, 90), r.Range(30, 90), r.Range(30, 90), r.Range(30, 90), r.Range(30, 90)));
        
        
        unit.transform.position = PosToWorldPos(unit.Pos.x, unit.Pos.y);
        unit.identify = identify;        

        return unitObject;
    }
    public GameObject CreateUnit(JsonData data)
    {
        GameObject unitObject = Instantiate(unitPrefabs[int.Parse(data[2].ToString())]);
        unitObject.name = String.Format("unit{0:D2}{1:D2}", int.Parse(data[0].ToString()), int.Parse(data[1].ToString()));
        SetPosInfoUnit(unitObject, int.Parse(data[0].ToString()), int.Parse(data[1].ToString()));
        Unit unit = unitObject.GetComponent<Unit>();

        unit.Init(unitObject.name, int.Parse(data[6].ToString()), int.Parse(data[5].ToString()), (Unit.CATEGORY)int.Parse(data[4].ToString()), 3, new Unit.Status(r.Range(30, 90), r.Range(30, 90), r.Range(30, 90), r.Range(30, 90), r.Range(30, 90), r.Range(30, 90)));


        unit.transform.position = PosToWorldPos(unit.Pos.x, unit.Pos.y);
        unit.identify = (Unit.IDENTIFY)int.Parse(data[3].ToString());

        return unitObject;
    }
    public void MoveUnit(Unit unit,int x, int y)
    {
        MoveUnit(unit, new Pos(x, y));

    }
    public void MoveUnit(Unit unit, Pos pos)
    {
        //유닛 위치 이동 컨트롤

        GameObject unitObject = unit.gameObject;
        unitObject.transform.position = PosToWorldPos(pos.x,pos.y);
        
        //Pos정보 수정
        location[unit.Pos].SetUnit();
        location[pos].SetUnit(unitObject);
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
