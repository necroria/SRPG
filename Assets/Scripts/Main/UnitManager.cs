using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    [SerializeField] List<GameObject> playerUnitPrefabs;
    [SerializeField] List<GameObject> commonUnitPrefabs;
    [SerializeField] List<GameObject> uncommonUnitPrefabs;
    static List<GameObject> playerUnitPool = new List<GameObject>();
    static List<GameObject> multiUserUnitPool = new List<GameObject>();
    static Dictionary<string, List<GameObject>> commonUnitPool = new Dictionary<string, List<GameObject>>();
    static Dictionary<string, List<GameObject>> uncommonUnitPool = new Dictionary<string, List<GameObject>>();
    static Dictionary<string, Unit.Status> defalutStat = new Dictionary<string, Unit.Status>();
    Transform tr;
    // Use this for initialization
    void Start () {
        InitDefalutStat();
        tr = GetComponent<Transform>();
        //아군 유닛풀 생성
        for(int i=0;i<playerUnitPrefabs.Count; i++)
        {
            playerUnitPool.Add(Instantiate<GameObject>(playerUnitPrefabs[i]));
        }
        //멀티시 필요한 유닛 생성
        for (int i = 0; i < playerUnitPrefabs.Count; i++)
        {
            multiUserUnitPool.Add(Instantiate<GameObject>(playerUnitPrefabs[i],tr));
            multiUserUnitPool[i].SetActive(false);
        }
        //적군으로 등장하는 잡몹 생성 10마리
        for (int i = 0; i < commonUnitPrefabs.Count; i++)
        {
            List<GameObject> units = new List<GameObject>();
            for(int j=0;j<10; j++)
            {
                units.Add(Instantiate<GameObject>(commonUnitPrefabs[i], tr));
                units[j].SetActive(false);
            }
            commonUnitPool.Add(commonUnitPrefabs[i].name,units);
        }
        //적군으로 등장하는 보스몹 생성 2마리
        for (int i = 0; i < uncommonUnitPrefabs.Count; i++)
        {
            List<GameObject> units = new List<GameObject>();
            for (int j = 0; j < 10; j++)
            {
                units.Add(Instantiate<GameObject>(uncommonUnitPrefabs[i], tr));
                units[j].name= uncommonUnitPrefabs[i].name;
                units[j].SetActive(false);
            }
            uncommonUnitPool.Add(uncommonUnitPrefabs[i].name, units);
        }

    }
    void InitDefalutStat()
    {
        defalutStat["footman"] = new Unit.Status(70, 60, 70, 30, 30, 40);
    }
	public static List<GameObject> GetPlayerUnitList()
    {
        return playerUnitPool;
    }
    /// <summary>
    /// player unit
    /// </summary>
    /// <param name="unitNumber"></param>
    /// <returns></returns>
    public static GameObject GetUnit(int unitNumber)
    {
        return multiUserUnitPool[unitNumber];
    }
    /// <summary>
    /// 1 : common, 2: uncommon
    /// </summary>
    /// <param name="unitType"></param>
    /// <returns></returns>
    public static GameObject GetUnit(int unitType, string key)
    {
        if(unitType == 1)
        {
            for(int i=0;i< commonUnitPool[key].Count; i++)
            {
                if (commonUnitPool[key][i].activeSelf==false)
                {
                    return commonUnitPool[key][i];
                }
            }
            //모든 유닛이 다 활성화될 경우 임시로 만들어 반환
            commonUnitPool[key].Add(Instantiate<GameObject>(commonUnitPool[key][0], commonUnitPool[key][0].GetComponent<Transform>().parent));
            return commonUnitPool[key][commonUnitPool[key].Count];
        }
        else if (unitType == 2)
        {
            for (int i = 0; i < uncommonUnitPool[key].Count; i++)
            {
                if (uncommonUnitPool[key][i].activeSelf == false)
                {
                    uncommonUnitPool[key][i].SetActive(true);
                    return uncommonUnitPool[key][i];
                }
            }
            //모든 유닛이 다 활성화될 경우 임시로 만들어 반환
            uncommonUnitPool[key].Add(Instantiate<GameObject>(uncommonUnitPool[key][0], uncommonUnitPool[key][0].GetComponent<Transform>().parent));
            return uncommonUnitPool[key][uncommonUnitPool[key].Count];
        }
        return null;
    }
    public static Unit.Status GetStat(string key)
    {
        return defalutStat[key];
    }
}
