using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTiles : MonoBehaviour {
    Map map;
    public GameObject tile;
    public List<Material> materials;
    Dictionary<Map.Pos, GameObject> tiles = new Dictionary<Map.Pos, GameObject>();
    public Dictionary<Map.Pos,List<Map.Pos>> hitPossiblePos = new Dictionary<Map.Pos, List<Map.Pos>>();
    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(Map map)
    {
        this.map = map;
        for(int i = 0; i < map.mapXSize; i++)
        {
            for(int j = 0; j < map.mapYSize; j++)
            {
                tiles.Add(new Map.Pos(i, j), Instantiate(tile,map.PosToWorldPos(i,j)+Vector3.up*0.01f,Quaternion.identity,transform));
            }
        }
    }
    public void ChangeTileMaterial(List<Map.Pos> posList,Unit.IDENTIFY identify)
    {
        
        int num = (int)identify;
        for (int i=0; i < posList.Count; i++)
        {
            tiles[posList[i]].GetComponent<Renderer>().sharedMaterial = materials[num];
            tilePosition(tiles[posList[i]], num);
            tiles[posList[i]].SetActive(true);
        }
    }
    public void ChangeTileMaterialHitPossible(Map.Pos pos)
    {
        tiles[pos].GetComponent<Renderer>().sharedMaterial = materials[materials.Count-1];
        tilePosition(tiles[pos], materials.Count - 1);
        tiles[pos].SetActive(true);
        
    }
    public void ChangeTileMaterialHitPossibleAttackRange(Unit unit)
    {

        List<Map.Pos> poslist = AttackRangeManager.Instance.GetPossiblePos(unit.range, unit);
        for (int i = 0; i < poslist.Count; i++)
        {

            if (!map.CheckVaildPos(poslist[i]))
                continue;
            GameObject unitObj = map.GetPosUnit(poslist[i]);
            if (unitObj == null)
                continue;

            if (unitObj.GetComponent<Unit>().identify == Unit.IDENTIFY.ENEMY)
            {
                tiles[poslist[i]].GetComponent<Renderer>().sharedMaterial = materials[materials.Count - 1];
                tilePosition(tiles[poslist[i]], materials.Count - 1);
                tiles[poslist[i]].SetActive(true);
            }
        }
    }
    public List<Map.Pos> ChangeTileMaterialHitPossibleSkillRange(Skill skill,Unit unit)
    {

        List<Map.Pos> poslist = SkillRangeManager.Instance.GetPossiblePos(skill.range, unit);
        List<Map.Pos> targetPosList = new List<Map.Pos>();
        for (int i = 0; i < poslist.Count; i++)
        {

            if (!map.CheckVaildPos(poslist[i]))
                continue;
            GameObject unitObj = map.GetPosUnit(poslist[i]);
            if (unitObj == null)
                continue;
            Unit targetUnit = unitObj.GetComponent<Unit>();
            //스킬사용자와 아군인 경우
            if(skill.type==Skill.SKILLTYPE.BUFF|| skill.type == Skill.SKILLTYPE.RECOVERY)
            {
                if (targetUnit.identify == unit.identify)
                {
                    tiles[poslist[i]].GetComponent<Renderer>().sharedMaterial = materials[materials.Count - 1];
                    tilePosition(tiles[poslist[i]], materials.Count - 1);
                    tiles[poslist[i]].SetActive(true);
                    targetPosList.Add(poslist[i]);
                }
            }
            if (skill.type == Skill.SKILLTYPE.OFFENSIVE || skill.type == Skill.SKILLTYPE.DEBUFF)
            {
                if (targetUnit.identify == Unit.IDENTIFY.ENEMY)
                {
                    tiles[poslist[i]].GetComponent<Renderer>().sharedMaterial = materials[materials.Count - 1];
                    tilePosition(tiles[poslist[i]], materials.Count - 1);
                    tiles[poslist[i]].SetActive(true);
                    targetPosList.Add(poslist[i]);
                }
            }
            
        }
        return targetPosList;
    }
    public void ClearTiles(List<Map.Pos> posList)
    {

        for (int i = 0; i < posList.Count; i++)
        {
            tiles[posList[i]].SetActive(false);
        }
    }
    public void ClearTiles()
    {
        
        Dictionary<Map.Pos, GameObject>.Enumerator e = tiles.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.Value.SetActive(false);            
        }
        
    }
    void tilePosition(GameObject tile,int num)
    {
        if (tile)
        {
            if (num == materials.Count - 1)
            {
                tile.transform.localPosition = new Vector3(tile.transform.localPosition.x, 0.02f, tile.transform.localPosition.z);
            }
            else
            {
                tile.transform.localPosition = new Vector3(tile.transform.localPosition.x, 0f, tile.transform.localPosition.z);
            }
        }
    }
}
