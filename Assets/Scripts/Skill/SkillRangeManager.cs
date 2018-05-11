using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRangeManager : MonoBehaviour {
    public enum SKILLRANGE
    {
        NONE=-1, SELF, THREE
    }
    class AttackRangeComparer : IEqualityComparer<SKILLRANGE>
    {
        public bool Equals(SKILLRANGE x, SKILLRANGE y)
        {
            return x == y;
        }

        public int GetHashCode(SKILLRANGE obj)
        {
            return (int)obj;
        }
    }
    private static SkillRangeManager instance = null;
    public static SkillRangeManager Instance
    {
        get { return instance; }
    }
    SKILLRANGE activeRange = SKILLRANGE.NONE;
    Dictionary<SKILLRANGE, GameObject> srDicObj = new Dictionary<SKILLRANGE, GameObject>(new AttackRangeComparer());
    Dictionary<SKILLRANGE, Range> arDic = new Dictionary<SKILLRANGE, Range>(new AttackRangeComparer());
    public List<GameObject> ars;
    //public List<Material> materials;
    // Use this for initialization
    void Start () {

        if (instance == null)
        {
            instance = this;
        }
        for(int i=0; i < ars.Count; i++)
        {
            GameObject temp = Instantiate<GameObject>(ars[i], this.transform);
            temp.GetComponent<Range>().Init();
            temp.SetActive(false);
            srDicObj.Add((SKILLRANGE)i, temp) ;
            arDic.Add((SKILLRANGE)i, temp.GetComponent<Range>());
        }
	}
	public GameObject GetAttackRange(SKILLRANGE range)
    {
        return srDicObj[range];
    }
    public void Clear()
    {
        if (activeRange == SKILLRANGE.NONE)
            return;
        srDicObj[activeRange].SetActive(false);
        activeRange = SKILLRANGE.NONE;
    }
    public void VisibleSkillRange(SKILLRANGE skillRange,Unit unit=null)
    {
        if (unit !=null) {
            //Debug.Log(unit);
            srDicObj[skillRange].SetActive(true);
            srDicObj[skillRange].transform.position = new Vector3(unit.transform.position.x, transform.position.y, unit.transform.position.z);           
            srDicObj[skillRange].GetComponent<Range>().CheckAndVisibleTiles(unit.Pos);
            activeRange = skillRange;
        }
        else
        {
            try
            {
                srDicObj[activeRange].SetActive(false);
                activeRange = SKILLRANGE.NONE;
            }
            catch (KeyNotFoundException)
            {
                if (activeRange != SKILLRANGE.NONE)
                {
                    throw;
                }                
            }
            
        }
    }
    public List<Map.Pos> GetPossiblePos(SKILLRANGE range, Unit unit)
    {        
        return arDic[range].getPos(unit.Pos);
    }

    public int GetMaxLength(SKILLRANGE range)
    {
        return srDicObj[range].GetComponent<Range>().maxLength;
    }
}
