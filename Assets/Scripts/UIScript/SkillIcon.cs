using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour {
    public int skillNum;
    //public SkillInfoPanel skillInfoPanel;
    RawImage rawImage;
	public void Init(int skillNum)
    {
        this.skillNum = skillNum;
        rawImage = GetComponent<RawImage>();

        rawImage.texture = Resources.Load<Texture>("SkillIcon/"+SkillManager.GetSkill(skillNum).image);
    }
    public Skill GetSkillInfo()
    {
        return SkillManager.GetSkill(skillNum);
    }
    public void ClickIcon()
    {
        SkillPanel.instance.infoPanel.SetInfo(GetSkillInfo(), rawImage.texture);
    }
}
