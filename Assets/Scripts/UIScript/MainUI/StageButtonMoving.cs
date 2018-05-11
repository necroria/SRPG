using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButtonMoving : MonoBehaviour {
    public Transform buttons;
    Transform tr;
	// Use this for initialization
	public void Init () {
        tr = GetComponent<Transform>();
        Transform[] transforms = buttons.GetComponentsInChildren<Transform>();
        int childCount = buttons.childCount;
        for (int i = 0; i< childCount; i++)
        {
            buttons.GetChild(0).parent = tr;
        }
        SelectBattlePanel sbP = GetComponentInParent<SelectBattlePanel>();
        for(int i=0; i < childCount; i++)
        {
            List<GameObject> chapters = new List<GameObject>();
            Transform cTr = tr.GetChild(i);
            for (int j = 0; j < cTr.childCount; j++)
            {
                chapters.Add(cTr.GetChild(j).gameObject);
            }
            sbP.chapterListInSeason.Add(chapters);
            
        }
        
    }
	
}
