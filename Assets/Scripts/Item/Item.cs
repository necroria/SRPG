using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    enum CATEGORY { WEAPON,ARMOR, ASSI,ETC }
    enum STATNUM {  HP,MP,ATK,DEF}
    // Use this for initialization
    public int hp;
    public int mp;
    public int atk;
    public int def;
    /// <summary>
    /// HP=0,MP=1,ATK=2,DEF=3
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public int GetStat(int num)
    {
        STATNUM statNum = (STATNUM)num;
        int result=0;
        switch (statNum)
        {
            case STATNUM.HP:
                result = hp;
                break;
            case STATNUM.MP:
                result = mp;
                break;
            case STATNUM.ATK:
                result = atk;
                break;
            case STATNUM.DEF:
                result = def;
                break;
        }
        return result;
    }
}
