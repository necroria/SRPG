using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;
public class BattleManager : MonoBehaviour {
    public MainCameraCtrl mcc;
    public Map map;
    public int stageNum;
    public MovingTiles mt;
    GameObject selectUnit;
    public bool IsSelectUnit
    {        
        get {            
                return selectUnit;
        }
    }
    int selectUnitIndex = -1;
    Map.Pos afterPos = new Map.Pos(-1,-1);
    MovableTileSearch mts;
    public int mapXSize;
    public int mapYSize;
    enum SELECTUNITSTATE {NONE,CLICKED,MOVED,ATTACKING,SKILL,SKILLACT,END }
    public enum UNITSTATE { ACT, END, DEAD }
    public List<UNITSTATE> allyUnitState;
    public int actUnitCount=0;
    public List<UNITSTATE> enemyUnitState;
    public BattleUI battleUI;
    public int curTurn =1;
    SELECTUNITSTATE selUnitState = SELECTUNITSTATE.NONE;
    private static BattleManager _instance = null;
    public static BattleManager instance
    {
        get { return _instance; }
    }
    List<Map.Pos> targetPosList;//스킬 타겟
    int actSkillNum=-1;
    private BattleManager(){}
    // Use this for initialization
    void Start () {
        stageNum = MainManager.playBattleNum;
        JsonData mapData = FileUtil.LoadMapData(MainManager.Instance.mapCode);
        if (mapData == null)
        {
            MainManager.LoadMainScene("데이터를 찾을 수 없습니다.");
        }
        else
        {
            map.Init(mapData);            
            mt.Init(map);
            mts = new MovableTileSearch(map);
            mapXSize = map.mapXSize;
            mapYSize = map.mapYSize;
            //for(int i = 0; i < map.unitAllyList.Count; i++)
            //{
            //    Debug.Log(map.unitAllyList[i].GetComponent<Unit>().Pos);
            //}
            battleUI.Init(map.maxTurn);
            if (!_instance)
            {
                _instance = this;
            }

            allyUnitState = new List<UNITSTATE>();
            enemyUnitState = new List<UNITSTATE>();
            Debug.Log(map.unitAllyList.Count);
            for (int i = 0; i < map.unitAllyList.Count; i++)
            {
                allyUnitState.Add(UNITSTATE.ACT);
            }
            for (int i = 0; i < map.unitEnemyList.Count; i++)
            {
                enemyUnitState.Add(UNITSTATE.ACT);
            }
            mcc.Init(this);
            actUnitCount = allyUnitState.Count;
        }
    }
    Vector3 touchedPos;
    Vector3 touchScreenPoint;
    bool touchOn=false;
    float touchTime = 0f;
    bool touchDelta = false;
	// Update is called once per frame
	void Update () {
       
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchScreenPoint = Input.mousePosition;
            touchOn = true;
        }
        if (touchOn)
        {
            touchTime += Time.unscaledDeltaTime;
            touchDelta = (touchScreenPoint - Input.mousePosition).magnitude>0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                
                if(touchTime<1f&&!touchDelta)
                    ClickEvent();
                
            }
            touchOn = false;
            touchTime = 0;
            touchDelta = false;
        }
#else
        if (Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchedPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchScreenPoint = touch.position;
                touchOn = true;
                
            }
            if (touchOn)
            {
                Vector3 tempPos = touch.position;
                touchTime += Time.unscaledDeltaTime;
                touchDelta = (touchScreenPoint - tempPos).magnitude > 0;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (EventSystem.current.IsPointerOverGameObject(0) == false)
                {
                    if ( touchTime < 1f&&!touchDelta)
                        ClickEvent();
                }
                touchOn = false;
                touchTime = 0;
                touchDelta = false;
            }

        }
#endif
    }
    //이동 함수
    IEnumerator Move(List<Map.Pos> posList,Unit unit)
    {
        AttackRangeManager.Instance.VisibleAttackRange();
        for (int i = 0; i < posList.Count; i++)
        {
			//Debug.Log (posList [i]);
            map.MoveUnit(unit, posList[i],false);
            
            yield return new WaitForSeconds(.01f);
			//yield return new WaitForEndOfFrame();
        }
        map.SetPosInfoUnit(unit.gameObject, posList[posList.Count - 1]);
        AttackRangeManager.Instance.VisibleAttackRange(unit);
        
        battleUI.btnPanel.SetActiveButton(BtnPanel.BUTTONKIND.ATTACK, CheckHitPossible(unit));
    }
    //이동 후 공격
    IEnumerator MoveAttack(List<Map.Pos> posList, Unit unit,Unit hitObjectUnit)
    {
        for (int i = 0; i < posList.Count; i++)
        {
            //Debug.Log (posList [i]);
            map.MoveUnit(unit, posList[i],false);

            yield return new WaitForSeconds(.01f);
            //yield return new WaitForEndOfFrame();
        }
        map.SetPosInfoUnit(unit.gameObject, posList[posList.Count - 1]);
        unit.UnitAnimCtrl.Attack(hitObjectUnit);
        UnitStateChange(selectUnitIndex, UNITSTATE.END);
        ClickUnitStateChange(SELECTUNITSTATE.NONE);
    }
    //타일 온오프
    void VisibleTiles(Unit unit=null)
    {
        //mt.ClearTiles(mts.GetMovablePosList());
        mt.ClearTiles();
        mt.hitPossiblePos.Clear();
        mts.ClearPos();
        AttackRangeManager.Instance.VisibleAttackRange();


        if (unit != null)
        {
            mts.SearchTiles(unit);
            mt.ChangeTileMaterial(mts.GetMovablePosList(), unit.Identify);

            HashSet<Map.Pos> posSet = mts.GetMovablePosSet();
            posSet.Add(unit.Pos);
            if (unit.identify == Unit.IDENTIFY.ALLY)
            {
                int hitMaxLength = AttackRangeManager.Instance.GetMaxLength(unit.range) + unit.movePoint;
                
                RaycastHit[] hits = Physics.SphereCastAll(unit.transform.position, hitMaxLength, unit.transform.forward);
                
                
                for (int i = 0; i < hits.Length; i++)
                {
                    GameObject hitObj = null;
                    if (hits[i].collider.CompareTag("UNIT"))
                    {
                        hitObj = hits[i].collider.transform.parent.gameObject;
                        Unit tempUnit = hitObj.GetComponent<Unit>();
                        if (tempUnit.identify != Unit.IDENTIFY.ENEMY)
                            continue;  
                        if(hitMaxLength<Map.Pos.GetLength(unit.Pos,tempUnit.Pos))
                            continue;
                        
                        List<Map.Pos> hitPosList = AttackRangeManager.Instance.GetPossiblePos(unit.range, tempUnit);
                        
                        for (int j = hitPosList.Count-1; j >= 0; j--)
                        {
                            if (!posSet.Contains(hitPosList[j]))
                            {
                                hitPosList.RemoveAt(j);
                            }
                        }
                        if (hitPosList.Count > 0)
                        {
                            //수정해야됨
                            mt.hitPossiblePos.Add(tempUnit.Pos,hitPosList);
                            mt.ChangeTileMaterialHitPossible(tempUnit.Pos);
                        }
                    }
                }
            }
        }
        else
        {

            selUnitState = SELECTUNITSTATE.NONE;
            selectUnit = null;
            selectUnitIndex = -1;
        }
        AttackRangeManager.Instance.VisibleAttackRange(unit);
    }
    //클릭했을 때
    void ClickEvent()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (selUnitState == SELECTUNITSTATE.SKILL)
            {
                ClickUnitStateChange(SELECTUNITSTATE.CLICKED);
                return;
            }
            if(selUnitState == SELECTUNITSTATE.SKILLACT)
            {
                ClickSkillAct(hitInfo);
                return;
            }
            if (hitInfo.collider.CompareTag("UNIT"))
            {
                ClickUnit(hitInfo);
                
            }
            else if (hitInfo.collider.name == "Map")
            {
                ClickMap(hitInfo);
                
            }
            else if (hitInfo.collider.CompareTag("DESTROY"))
            {
                Debug.Log("hitDestory");
                switch (selUnitState)
                {
                    case SELECTUNITSTATE.NONE:
                        break;
                    case SELECTUNITSTATE.CLICKED:
                        ClickUnitStateChange(SELECTUNITSTATE.NONE);
                        break;
                    case SELECTUNITSTATE.MOVED:
                        ClickUnitStateChange(SELECTUNITSTATE.CLICKED);
                        break;
                    case SELECTUNITSTATE.ATTACKING:
                        ClickUnitStateChange(SELECTUNITSTATE.CLICKED);
                        break;
                    case SELECTUNITSTATE.SKILL:
                        break;
                    case SELECTUNITSTATE.SKILLACT:
                        break;
                    case SELECTUNITSTATE.END:
                        break;
                }          
            }
        }
    }

    private void ClickMap(RaycastHit hitInfo)
    {
        //선택된 유닛이 있을 때
        if (selectUnit)
        {
            Unit unit = selectUnit.GetComponent<Unit>();
            if (unit.Identify == Unit.IDENTIFY.ALLY && selUnitState == SELECTUNITSTATE.CLICKED)
            {
                if (allyUnitState[selectUnitIndex] == UNITSTATE.END)
                {
                    ClickUnitStateChange(SELECTUNITSTATE.NONE);
                    return;
                }
                if (mts.CheckMovablePos(map.WorldPosToMapPos(hitInfo.point)))
                {
                    
                    ClickUnitStateChange(SELECTUNITSTATE.MOVED);
                    StartCoroutine(Move(mts.GetMoveLinePos(map.WorldPosToMapPos(hitInfo.point)), unit));                    
                    return;
                }
            }
            else if (unit.Identify == Unit.IDENTIFY.ALLY && selUnitState == SELECTUNITSTATE.MOVED || unit.Identify == Unit.IDENTIFY.ALLY && selUnitState == SELECTUNITSTATE.ATTACKING)
            {
                if(Map.Pos.ValidInBattle(afterPos))
                    map.MoveUnit(unit, afterPos);
                ClickUnitStateChange(SELECTUNITSTATE.CLICKED);
                return;
            }
            ClickUnitStateChange(SELECTUNITSTATE.NONE);
            return;
        }
    }
    void ClickUnit(RaycastHit hitInfo)
    {
        GameObject hitObject = hitInfo.collider.transform.parent.gameObject;
        Unit hitObjectUnit = hitObject.GetComponent<Unit>();

        if (selectUnit)
        {
            //동일 유닛 체크
            if (selectUnit.Equals(hitObject))
            {
                
                if (selUnitState != SELECTUNITSTATE.CLICKED)
                    return;                
                ClickUnitStateChange(SELECTUNITSTATE.NONE);
                return;
            }
            else
            {
                Unit unit = selectUnit.GetComponent<Unit>();                
                
                if (unit.Identify == Unit.IDENTIFY.ALLY && hitObjectUnit.Identify == Unit.IDENTIFY.ENEMY)
                {
                    if (allyUnitState[selectUnitIndex] == UNITSTATE.END)
                    {
                        return;
                    }
                    if (mt.hitPossiblePos.ContainsKey(hitObjectUnit.Pos))
                    {
                        List<Map.Pos> hitPosList = mt.hitPossiblePos[hitObjectUnit.Pos];
                        switch (selUnitState)
                        {              
                            case SELECTUNITSTATE.CLICKED:
                                if (hitPosList.Contains(selectUnit.GetComponent<Unit>().Pos))
                                {
                                    InPlaceAttack(unit,hitObjectUnit);
                                }
                                else
                                {
                                    Map.Pos hitPos = GetHitPos(hitPosList, hitObjectUnit, unit);
                                    StartCoroutine(MoveAttack(mts.GetMoveLinePos(hitPos), unit,hitObjectUnit));
                                }
                                return;
                                
                            case SELECTUNITSTATE.MOVED:
                                if (hitPosList.Contains(selectUnit.GetComponent<Unit>().Pos))
                                {
                                    InPlaceAttack(unit,hitObjectUnit);
                                    return;
                                }
                                break;
                            case SELECTUNITSTATE.ATTACKING:
                                if (hitPosList.Contains(selectUnit.GetComponent<Unit>().Pos))
                                {
                                    InPlaceAttack(unit, hitObjectUnit);
                                    return;
                                }
                                break;
                            case SELECTUNITSTATE.SKILL:
                                break;
                            default:
                                break;
                        }                        
                    }

                }
                if (Map.Pos.ValidInBattle(afterPos))
                    map.MoveUnit(unit, afterPos);
            }
        }
        
        selectUnit = hitObject;
        ClickUnitStateChange(SELECTUNITSTATE.CLICKED);  

    }
    void ClickSkillAct(RaycastHit hitInfo)
    {
        Unit selUnit = selectUnit.GetComponent<Unit>();
        if (hitInfo.collider.CompareTag("UNIT"))
        {
            Unit targetUnit = hitInfo.collider.gameObject.GetComponentInParent<Unit>();
            //범위내 대상인지 확인 후 대상이면 적용 아니면 캐릭터 선택 상태로
            Debug.Log(targetPosList);
            Debug.Log(targetUnit);
            if (targetPosList.Contains(targetUnit.Pos))
            {
                SkillManager.ActSkill(actSkillNum, selUnit, targetUnit);
                SkillRangeManager.Instance.Clear();
                VisibleTiles(); 
            }
            else
            {
                ClickUnit(hitInfo);
            }
        }
        if (hitInfo.collider.name == "Map")
        {
            //범위 내 인지 확인 후 아니면 캐릭터 선택 상태로 맞으면 암것도 안함
            bool contain=SkillRangeManager.Instance.GetPossiblePos(SkillManager.GetSkill(actSkillNum).range,selUnit).Contains(map.WorldPosToMapPos(hitInfo.point));
            if (!contain)
                ClickUnitStateChange(SELECTUNITSTATE.SKILL);
            
        }
        if (hitInfo.collider.CompareTag ("DESTROY"))
        {
            ClickUnitStateChange(SELECTUNITSTATE.SKILL);
        }
    }
    private Map.Pos GetHitPos(List<Map.Pos> hitPosList, Unit hitUnit, Unit unit)
    {
        Map.Pos selUnitPos = unit.Pos;
        Map.Pos hitUnitPos = hitUnit.Pos;
        
        List<Map.Pos> hitUnitRangePos = AttackRangeManager.Instance.GetPossiblePos(hitUnit.range, hitUnit);

        List < Map.Pos > interPosList = Enumerable.Intersect(hitPosList, hitUnitRangePos).ToList<Map.Pos>();
        hitPosList.Sort((Map.Pos a, Map.Pos b) => Map.Pos.GetLength(a, selUnitPos).CompareTo(Map.Pos.GetLength(b, selUnitPos)));
        
        for(int i=0;i< hitPosList.Count; i++)
        {
            if(!interPosList.Contains(hitPosList[i]))
                return hitPosList[i];
        }
        
        interPosList.Sort((Map.Pos a, Map.Pos b) => Map.Pos.GetLength(a, selUnitPos).CompareTo(Map.Pos.GetLength(b, selUnitPos)));
        return interPosList[0];
    }

    public bool CheckHitPossible(Unit unit)
    {
        List<Map.Pos> poslist = AttackRangeManager.Instance.GetPossiblePos(unit.range, unit);
        for(int i = 0; i < poslist.Count; i++)
        {
            
            if (!map.CheckVaildPos(poslist[i]))
                continue;
            GameObject unitObj = map.GetPosUnit(poslist[i]);
            if (unitObj == null)
                continue;
            
            if (unitObj.GetComponent<Unit>().identify == Unit.IDENTIFY.ENEMY)
            {
                return true;
            }
        }
        return false;
    }

    public void Wait()
    {
        UnitStateChange(selectUnitIndex, UNITSTATE.END);
        ClickUnitStateChange(SELECTUNITSTATE.NONE);
    }
    public void Attack()
    {
        ClickUnitStateChange(SELECTUNITSTATE.ATTACKING); 
    }
    public void Skill()
    {
        ClickUnitStateChange(SELECTUNITSTATE.SKILL);
        Unit unit = selectUnit.GetComponent<Unit>();
        //SkillPanel.instance.gameObject.SetActive(true);
        
        //Debug.Log(SkillPanel.instance);
        SkillPanel.instance.OnSkillPanel(unit,true);
    }
    public void TurnEnd()
    {
        
        for (int i = 0; i < allyUnitState.Count; i++)
        {
            if (allyUnitState[i] != UNITSTATE.DEAD)
            {
                if(allyUnitState[i]!= UNITSTATE.END)
                {
                    //수정 필요
                    allyUnitState[i] = UNITSTATE.END;
                    actUnitCount--;
                    
                }                    
            }

        }
        //적턴으로 가는 함수 필요
        TurnStart();
    }
    void TurnStart()
    {
        for(int i = 0; i < allyUnitState.Count; i++)
        {
            if (allyUnitState[i] != UNITSTATE.DEAD)
            {                
                UnitStateChange(i, UNITSTATE.ACT);
                
            }        
            
        }
        for (int i = 0; i < allyUnitState.Count; i++)
        {
           Debug.Log(allyUnitState[i]);
        }
        battleUI.battleInfoPanel.SetTurn(++curTurn);
        
    }
    /// <summary>
    /// 
    /// 아군유닛의 경우만 적용됨 적군까지 컨트롤이 필요할 수 있음
    /// 
    /// </summary>
    void UnitStateChange(int index,UNITSTATE unitState)
    {
        allyUnitState[index] = unitState;
        //
        if (unitState == UNITSTATE.ACT)
        {
            actUnitCount++;
        }
        if(unitState == UNITSTATE.DEAD)
        {
            //
        }
        if(unitState == UNITSTATE.END)
        {
            actUnitCount--;
        }

        if (actUnitCount == 0)
        {
            //턴종료
            //임시
            if (curTurn == map.maxTurn)
            {
                //패배
                return;
            }
            TurnStart();
            
        }
    }
    //제자리 공격
    void InPlaceAttack(Unit unit, Unit hitObjectUnit)
    {
        unit.UnitAnimCtrl.Attack(hitObjectUnit);
        
        UnitStateChange(selectUnitIndex, UNITSTATE.END);
        ClickUnitStateChange(SELECTUNITSTATE.NONE);
    }
    public void ActSkill(int skillNum)
    {
        targetPosList = null;
        actSkillNum = skillNum;
        ClickUnitStateChange(SELECTUNITSTATE.SKILLACT);
        mt.ClearTiles();
        AttackRangeManager.Instance.VisibleAttackRange();
        battleUI.btnPanel.SetActive(false);
        SkillPanel.instance.gameObject.SetActive(false);
        Skill skill = SkillManager.GetSkill(skillNum);
        Unit unit = selectUnit.GetComponent<Unit>();
        
        switch (skill.scope)
        {
            
            case global::Skill.SKILLSCOPE.ONE:
                //개인 대상은 스킬 범위, 적용 가능 대상 보여준 후 대상 클릭 시 사용
                SkillRangeManager.Instance.VisibleSkillRange(skill.range, unit);
                targetPosList = mt.ChangeTileMaterialHitPossibleSkillRange(skill, unit);
                
                break;
            case global::Skill.SKILLSCOPE.ALL:
                //전체 대상은 사용여부가능 후 체크 후 바로 사용
                break;
            case global::Skill.SKILLSCOPE.AROUND:
                //주위 범위는 스킬 범위 적용 대상 보여준 후 사용 버튼 클릭해서 사용
                SkillRangeManager.Instance.VisibleSkillRange(skill.range, unit);
                targetPosList = mt.ChangeTileMaterialHitPossibleSkillRange(skill, unit);
                break;
        }
    }
    void ClickUnitStateChange(SELECTUNITSTATE state)
    {
        if (selectUnit == null)
            return;
        Unit unit = selectUnit.GetComponent<Unit>();
        switch (state)
        {

            case SELECTUNITSTATE.NONE:

                battleUI.SetActivePanel(false);
                SkillPanel.instance.OffSkillPanel();
                VisibleTiles();
                break;
            case SELECTUNITSTATE.CLICKED:
                afterPos = new Map.Pos(-1, -1);
                battleUI.SetActivePanel(true, unit);
                SkillPanel.instance.OffSkillPanel();
                if (unit.identify == Unit.IDENTIFY.ALLY)
                {                    
                    selectUnitIndex = map.unitAllyList.IndexOf(selectUnit);
                    if (allyUnitState[selectUnitIndex] == UNITSTATE.ACT)
                    {
                        battleUI.btnPanel.SetActiveButton(BtnPanel.BUTTONKIND.SKILL, unit.skillList.Count > 0);
                        battleUI.btnPanel.SetActiveButton(BtnPanel.BUTTONKIND.ATTACK, CheckHitPossible(unit));
                    }
                    else
                    {
                        battleUI.btnPanel.SetActive(false);
                    }
                }
                VisibleTiles(unit);
                break;
            case SELECTUNITSTATE.MOVED:
                afterPos = unit.Pos;
                mt.ClearTiles();
                mt.ChangeTileMaterialHitPossibleAttackRange(unit);
                break;
            case SELECTUNITSTATE.ATTACKING:

                battleUI.btnPanel.SetActive(false);

                mt.ClearTiles();
                mt.ChangeTileMaterialHitPossibleAttackRange(unit);
                break;
            case SELECTUNITSTATE.SKILL:
                //스킬창 보여주고 있는 상태              
               

                //Debug.Log(SkillPanel.instance);
                SkillPanel.instance.OnSkillPanel(unit, true);
                SkillRangeManager.Instance.Clear();
                break;
            case SELECTUNITSTATE.SKILLACT:
                //스킬창에서 스킬 사용을 누른 상태
                battleUI.SetActivePanel(false);
                SkillPanel.instance.OffSkillPanel();
                break;
            case SELECTUNITSTATE.END:
                break;
            default:
                break;

        }
        selUnitState = state;
    }
}