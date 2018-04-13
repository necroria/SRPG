using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitInfoPanel : MyPanel {
    public RectTransform thumbnailRtr;
    public RawImage thumbnail;
    public Text info;
    public Texture tempImage;
    // Use this for initialization
    void Start () {
        RectTransform rtr = GetComponent<RectTransform>();
        thumbnailRtr.sizeDelta = new Vector2(rtr.rect.size.y-6, rtr.rect.size.y);
        
        

        
    }
	
    public void SetUnitInfo(Unit unit)
    {
        if (!unit.image)
        {
            thumbnail.texture = tempImage;
        }
        else
        {
            thumbnail.texture = unit.image;
        }
        
        info.text = TextForm(unit);
    }
    string TextForm(Unit unit)
    {
        return "이름 : " + unit.name + " \t직업 : " + UnitCategoryManager.GetInfo(unit.category,unit.rank).rankName + " \t체력 : "+unit.hp+" / " + unit.maxHp+"\n공격력 : "+unit.atk+ " \t방어력 : " + unit.def;
    }
}
