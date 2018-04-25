using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill {
    public string image;
    public string name;
    public int skillNum;
    public string effect;
    public string comment;
    int limitLevel;
    public SkillRangeManager.SKILLRANGE range;
    public SKILLTYPE type;
    public SKILLSCOPE scope;
    public enum SKILLNUM
    {
        NONE=-1,SELFRECOVERY, PENETRATIONATTACK
    }
    public enum SKILLTYPE
    {
        OFFENSIVE, BUFF, RECOVERY,DEBUFF,ETC
    }
    public enum SKILLSCOPE
    {
        ONE, ALL, AROUND 
    }
	
    public void Init(string image,string name, int skillNum,string effect,string comment,int limitLevel,SKILLTYPE type, SkillRangeManager.SKILLRANGE range, SKILLSCOPE scope)
    {
        this.image = image;
        this.name = name;
        this.skillNum = skillNum;
        this.effect = effect;
        this.comment = comment;
        this.limitLevel = limitLevel;
        this.type = type;
        this.range = range;
        this.scope = scope;
    }
}
