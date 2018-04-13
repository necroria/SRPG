using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour {
    public AttackRangeManager.ATTACKRANGE range;
    HashSet<Transform> tiles = new HashSet<Transform>();
    public List<Map.Pos> posList = new List<Map.Pos>();
    public int maxLength=0;
	
	public void Init()
    {
        Transform[] trArray = GetComponentsInChildren<Transform>();
        for (int i = 1; i < trArray.Length; i++)
        {
            int tempLength = Mathf.Abs((int)trArray[i].localPosition.x) + Mathf.Abs((int)trArray[i].localPosition.z);
            if (maxLength<tempLength)
            {
                maxLength = tempLength;
            }
            tiles.Add(trArray[i]);
            posList.Add(new Map.Pos(-(int)trArray[i].localPosition.z, (int)trArray[i].localPosition.x));
        }
    }
    public void CheckAndVisibleTiles(Map.Pos unitPos)
    {
        
        Map map = BattleManager.instance.map;
        
        HashSet<Transform>.Enumerator e = tiles.GetEnumerator();
        while (e.MoveNext())
        {
            
            e.Current.gameObject.SetActive(VaildCheck(new Map.Pos(-(int)e.Current.localPosition.z, (int)e.Current.localPosition.x), unitPos, map));
        }

    }
    bool VaildCheck(Map.Pos pos, Map.Pos unitPos, Map map)
    {
        //Debug.Log(pos);
        Map.Pos newPos = pos + unitPos;
        
        return !(newPos.x<0||newPos.x>=map.mapXSize|| newPos.y < 0 || newPos.y >= map.mapYSize);
    }

    public List<Map.Pos> getPos(Map.Pos unitPos)
    {
        List<Map.Pos> tempList = new List<Map.Pos>();
        for(int i=0; i < posList.Count; i++)
        {
            tempList.Add(unitPos + posList[i]);
        }
        return tempList;
        
    }
}
