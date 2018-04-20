using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    static List<Skill> skillList = new List<Skill>();
    
	// Use this for initialization
	void Start () {
		
	}

    public static Skill GetSkill(int skillNum)
    {
        return skillList[skillNum];
    }
}
