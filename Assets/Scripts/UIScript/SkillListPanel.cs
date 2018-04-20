using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListPanel : MonoBehaviour {
    GridLayoutGroup gl;
    RectTransform rtr;
    public int CountPerLine = 5;
    // Use this for initialization
    void Start () {
        gl = GetComponent<GridLayoutGroup>();
        rtr = GetComponent<RectTransform>();
        float cellSize = (rtr.rect.width-(gl.padding.left+gl.padding.right)) / CountPerLine - gl.spacing.y;
        gl.cellSize = new Vector2(cellSize, cellSize);
        int ch = GetComponentsInChildren<RawImage>().Length;
        int line = (ch / CountPerLine) +((ch % CountPerLine) >0?1:0);
        //rtr.sizeDelta = new Vector2(0, cellSize*line*(line+1)*5);
        rtr.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize * line + (line - 1) * CountPerLine + gl.padding.top+gl.padding.bottom) ;
        //rtr.anchoredPosition = Vector2.zero;
    }
	
}
