using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCategoryManager : MonoBehaviour {
    //static UnitCategoryManager _instance = null;
    //public static UnitCategoryManager instance
    //{
    //    get
    //    {            
    //        return _instance;
    //    }
    //}
    private UnitCategoryManager() { }
    static Dictionary<Unit.CATEGORY, UnitCategory> categoryDic = new Dictionary<Unit.CATEGORY, UnitCategory>();
    public List<UnitCategory.Info> arli = new List<UnitCategory.Info>();
    public List<UnitCategory.Info> kili = new List<UnitCategory.Info>();
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start() {
        Debug.Log("start UnitCategoryManager");
        //if (!_instance)
        //{
        //    _instance = this;
        //}
        categoryDic[Unit.CATEGORY.KING] = new UnitCategory();
        List<UnitCategory.Info> kingList = new List<UnitCategory.Info>();
        UnitCategory.Info infoKing = new UnitCategory.Info();
        infoKing.atkUp = 0.1f;
        infoKing.consumingMovePoint = new int[] { 1, 2 };
        infoKing.defUp = 0.1f;
        infoKing.hp = 5;
        infoKing.mp = 3;
        infoKing.rankName = "왕";
        infoKing.movePoint = 7;
        infoKing.range = AttackRangeManager.ATTACKRANGE.ONE;
        
        kingList.Add(infoKing);
        UnitCategory.Info infoEmperor = new UnitCategory.Info();
        infoEmperor.atkUp = 0.15f;
        infoEmperor.consumingMovePoint = new int[] { 1, 2 };
        infoEmperor.defUp = 0.15f;
        infoEmperor.hp = 6;
        infoEmperor.mp = 3;
        infoEmperor.rankName = "황제";
        infoEmperor.movePoint = 8;
        infoEmperor.range = AttackRangeManager.ATTACKRANGE.ONE;
        infoEmperor.skillNum = new int[] { 0 };
        kingList.Add(infoEmperor);
        categoryDic[Unit.CATEGORY.KING].Set(Unit.CATEGORY.KING, kingList);

        categoryDic[Unit.CATEGORY.ARCHER] = new UnitCategory();
        List<UnitCategory.Info> archerList = new List<UnitCategory.Info>();
        UnitCategory.Info infoArcher = new UnitCategory.Info();
        infoArcher.atkUp = 0.13f;
        infoArcher.consumingMovePoint = new int[] { 1, 1 };
        infoArcher.defUp = 0.05f;
        infoArcher.hp = 4;
        infoArcher.mp = 3;
        infoArcher.rankName = "아처";
        infoArcher.movePoint = 5;
        infoArcher.range = AttackRangeManager.ATTACKRANGE.TWO;
        infoArcher.skillNum = new int[] { 1 };
        archerList.Add(infoArcher);
        UnitCategory.Info infoRanger = new UnitCategory.Info();
        infoRanger.atkUp = 0.18f;
        infoRanger.consumingMovePoint = new int[] { 1, 1 };
        infoRanger.defUp = 0.07f;
        infoRanger.hp = 5;
        infoRanger.mp = 3;
        infoRanger.rankName = "레인저";
        infoRanger.movePoint = 5;
        infoRanger.range = AttackRangeManager.ATTACKRANGE.THREE;
        infoRanger.skillNum = new int[] { 1 };
        archerList.Add(infoRanger);
        categoryDic[Unit.CATEGORY.ARCHER].Set(Unit.CATEGORY.ARCHER, archerList);
        kili=(kingList);
        arli=(archerList);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public static UnitCategory GetCategory(Unit.CATEGORY category)
    {
        return categoryDic[category];
    }
    public static UnitCategory.Info GetInfo(Unit.CATEGORY category,int rank)
    {
        
        return categoryDic[category].GetInfo(rank);
    }

}
