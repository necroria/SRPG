using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillInfoPanel : MonoBehaviour {
    
    public RawImage skillImage;
    public Text skillName;
    public Text skillEffect;
    public Text skillComment;
    public void SetInfo(Skill skill,Texture skillIcon)
    {
        skillImage.texture = skillIcon;
        skillName.text = skill.name;
        skillEffect.text = skill.effect;
        skillComment.text = skill.comment;
    }
    public void Clear()
    {
        skillImage.texture = null;
        skillName.text = "";
        skillEffect.text = "";
        skillComment.text = "";
    }
}
