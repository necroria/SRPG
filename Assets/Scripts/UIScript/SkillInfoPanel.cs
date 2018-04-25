using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillInfoPanel : MonoBehaviour {
    
    public RawImage skillImage;
    public Text skillName;
    public Text skillEffect;
    public Text skillComment;
    public RectTransform skillImageRtr;
    public RectTransform skillNameRtr;
    public RectTransform skillEffectRtr;
    public RectTransform skillCommentRtr;
    int skillNum = -1;
    public void SetInfo(Skill skill,Texture skillIcon)
    {
        skillNum = skill.skillNum;
        skillImage.texture = skillIcon;
        skillImage.color = new Color(255, 255, 255, 255);
        skillName.text = skill.name;
        skillName.resizeTextForBestFit = true;
        skillNameRtr.anchoredPosition = -Vector2.up*skillImageRtr.sizeDelta.y;
        skillEffect.text = skill.effect;
        skillEffect.resizeTextForBestFit = true;
        skillEffectRtr.anchoredPosition = -Vector2.up * (skillImageRtr.sizeDelta.y+ skillNameRtr.sizeDelta.y);
        skillComment.text = skill.comment;
        skillComment.resizeTextForBestFit = true;
        skillCommentRtr.anchoredPosition = -Vector2.up * (skillImageRtr.sizeDelta.y + skillNameRtr.sizeDelta.y+ skillEffectRtr.sizeDelta.y);
    }
    public void Clear()
    {
        skillImage.texture = null;
        skillImage.color = new Color(255, 255, 255, 0);
        skillName.text = "";
        skillEffect.text = "";
        skillComment.text = "";
        skillNum = -1;
    }
    public void ClickSkillActButton()
    {
        BattleManager.instance.ActSkill(skillNum);
    }
}
