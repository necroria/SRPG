using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListPanel : MonoBehaviour {
    public GridLayoutGroup gl;
    public RectTransform rtr;
    public Transform iconParent;
    public Transform contentTr;
    public int CountPerLine = 5;
    bool setCellSize = false;
    float cellSize;
    
    
    public void AddSkillIcon(Unit unit)
    {
        //임시
        if (!setCellSize)
        {

            cellSize = (GetComponent<RectTransform>().sizeDelta.x - (gl.padding.left + gl.padding.right)) / CountPerLine - gl.spacing.y;
            gl.cellSize = new Vector2(cellSize, cellSize);
            setCellSize = true;
        }
        Debug.Log(transform.childCount);
        for(int i = contentTr.childCount-1; i >= 0; i--)
        {
            contentTr.GetChild(i).SetParent(iconParent);
        }
        
        for(int i = 0; i < unit.skillList.Count; i++)
        {
            SkillManager.GetSkillIcon(unit.skillList[i]).transform.SetParent(contentTr);

        }
        int ch = GetComponentsInChildren<RawImage>().Length;
        int line = (ch / CountPerLine) + ((ch % CountPerLine) > 0 ? 1 : 0);
        //rtr.sizeDelta = new Vector2(0, cellSize*line*(line+1)*5);

        rtr.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize * line + (line - 1) * CountPerLine + gl.padding.top + gl.padding.bottom);


    }

}
