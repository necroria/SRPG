using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnPanel : MyPanel {

    GridLayoutGroup gl;
    RectTransform rtr;
    public enum BUTTONKIND { ATTACK,SKILL,ITEM,WAIT}
    public List<GameObject> buttons;
    
    Unit unit;
	// Use this for initialization
	void Start () {
        
        gl = GetComponent<GridLayoutGroup>();
        rtr = GetComponent<RectTransform>();
        gl.cellSize = new Vector2(rtr.rect.width / 4, rtr.rect.height);

        
    }
	public void SetActiveButton(BUTTONKIND kind,bool value)
    {

        int num = (int)kind;
        SetActiveButton(num, value);
    }
    public void SetActiveButton(int num, bool value)
    {
   
        buttons[num].GetComponent<Image>().enabled = value;
        buttons[num].GetComponentInChildren<Text>().enabled = value;        
    }
    public void Attack()
    {
        BattleManager.instance.Attack();
    }
    public void Skill()

    {

    }
    public void Item()
    {

    }
    public void Wait()
    {
        BattleManager.instance.Wait();
    }
}
