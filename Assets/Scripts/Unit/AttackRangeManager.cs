using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeManager : MonoBehaviour {
    public enum ATTACKRANGE
    {
        NONE=-1,ONE, TWO, THREE
    }
    class AttackRangeComparer : IEqualityComparer<ATTACKRANGE>
    {
        public bool Equals(ATTACKRANGE x, ATTACKRANGE y)
        {
            return x == y;
        }

        public int GetHashCode(ATTACKRANGE obj)
        {
            return (int)obj;
        }
    }
    private static AttackRangeManager instance = null;
    public static AttackRangeManager Instance
    {
        get { return instance; }
    }
    ATTACKRANGE activeRange = ATTACKRANGE.NONE;
    Dictionary<ATTACKRANGE, GameObject> arDicObj = new Dictionary<ATTACKRANGE, GameObject>(new AttackRangeComparer());
    Dictionary<ATTACKRANGE, Range> arDic = new Dictionary<ATTACKRANGE, Range>(new AttackRangeComparer());
    public List<GameObject> ars;
    public List<Material> materials;
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
            arDicObj.Add((ATTACKRANGE)i, temp) ;
            arDic.Add((ATTACKRANGE)i, temp.GetComponent<Range>());
        }
	}
	public GameObject GetAttackRange(ATTACKRANGE range)
    {
        return arDicObj[range];
    }
    public void VisibleAttackRange(Unit unit=null)
    {
        if (unit !=null) {
            //Debug.Log(unit);
            arDicObj[unit.range].SetActive(true);
            arDicObj[unit.range].transform.position = new Vector3(unit.transform.position.x, transform.position.y, unit.transform.position.z);           
            arDicObj[unit.range].GetComponent<Range>().CheckAndVisibleTiles(unit.Pos);
            activeRange = unit.range;
        }
        else
        {
            try
            {
                arDicObj[activeRange].SetActive(false);
                activeRange = ATTACKRANGE.NONE;
            }
            catch (KeyNotFoundException)
            {
                if (activeRange != ATTACKRANGE.NONE)
                {
                    throw;
                }                
            }
            
        }
    }
    public List<Map.Pos> GetPossiblePos(ATTACKRANGE range, Unit unit)
    {        
        return arDic[range].getPos(unit.Pos);
    }

    public int GetMaxLength(ATTACKRANGE range)
    {
        return arDicObj[range].GetComponent<Range>().maxLength;
    }
}
