using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill : MonoBehaviour {
    public string image;
    public new string name;
    public int skillNum;
    public string effect;
    public string comment;
    int limitLevel;
    SKILLTYPE type;
    public enum SKILLNUM
    {
        NONE,
    }
    public enum SKILLTYPE
    {
        OFFENSIVE, BUFF, RECOVERY,ETC
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Init()
    {

    }
}
