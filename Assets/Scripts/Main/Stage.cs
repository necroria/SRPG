using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Stage {

    public int season;
    public int chapter;
    public int stage;
    public string name;
    public string info;
    
    public Stage(int season,int chapter, int stage, string name, string info)
    {
        this.season=season;
        this.chapter = chapter;
        this.stage = stage;
        this.name = name;
        this.info = info;
    }
    public override string ToString()
    {
        return season+" "+chapter+" "+name+info;
    }
}
