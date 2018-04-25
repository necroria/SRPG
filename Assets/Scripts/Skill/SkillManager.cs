using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    static List<Skill> skillList = new List<Skill>();
    static List<GameObject> skillIconList = new List<GameObject>();
    public GameObject skillIconPrefab;
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        Skill skill01 = new Skill();
        Skill skill02 = new Skill();
        skill01.Init("skill01", "skill01", 0, "skill01", "skill01",
            limitLevel:0,type:Skill.SKILLTYPE.RECOVERY,range:SkillRangeManager.SKILLRANGE.SELF,scope:Skill.SKILLSCOPE.ONE);
        skill02.Init("skill02", "skill02", 1, "skill02", "skill02",
            limitLevel: 0, type: Skill.SKILLTYPE.OFFENSIVE, range: SkillRangeManager.SKILLRANGE.THREE, scope: Skill.SKILLSCOPE.ONE);
        skillList.Add(skill01);
        skillList.Add(skill02);

        for(int i=0; i < skillList.Count; i++)
        {
            GameObject skillIcon = Instantiate<GameObject>(skillIconPrefab,this.transform);
            skillIcon.GetComponent<SkillIcon>().Init(i);
            skillIconList.Add(skillIcon);
        }
    }

    public static Skill GetSkill(int skillNum)
    {
        return skillList[skillNum];
    }
    public static GameObject GetSkillIcon(int skillNum)
    {
        return skillIconList[skillNum];
    }
    public static void ActSkill(Unit user,Unit[] target,int skillNum)
    {
        switch (skillNum)
        {
            case 0:
                int amount = Mathf.Clamp(user.stat.CON, 0, user.maxHp - user.hp);
                user.hp += amount;
                Debug.Log(amount + "회복되었습니다.");
                break;
            case 1:
                for(int i = 0; i < target.Length; i++)
                {
                    int damage = (int)(user.stat.DEX * (user.level / 5+1)*0.2f);
                    target[i].hp = Mathf.Clamp(target[i].hp,0,target[i].maxHp);                   
                }
                break;
        }
    }
}
