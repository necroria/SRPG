using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainManager : MonoBehaviour {
    private static MainManager _instance = null;
    public static MainManager Instance
    {
        get { return _instance; }
    }
    public static int playBattleNum = 1;
    public enum STATE { MAIN,UNITMANAGEMENT,SELECTBATTLE,BATTLE}
    STATE currentState = STATE.MAIN;
    public Text text;
    public Player player;
    public Transform[] mainUnitTr;
    public WaitPlace wp;
    public string mapCode;
    public int battleType;
    public enum BATTLECATEGORY { NORMAL, }
   
    private void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        player.Init();
        wp.Init(player.unitCount,player);
        SetWaitUnit();
        SetMainUnit();
        
    }
    public void LoadBattleScene(int battleType,string mapCode)
    {
        this.battleType = battleType;
        this.mapCode = mapCode;
        player.SetActiveAllUnit(false);
        SceneManager.LoadScene("Battle");
        
    }
    public static void LoadMainScene(string message)
    {
        SceneManager.LoadScene("Main");
        Debug.Log(message);
    }
    //private void OnGUI()
    //{
    //    //GUI.Box(new Rect(0, 0, 1000, 80), Application.dataPath+"\n"+FileUtil.dataPath(playBattleNum)+"\n"+ Application.streamingAssetsPath);
    //}
    public void SetBattleStageNum(int num)
    {
        playBattleNum = num;
        text.text = playBattleNum + "Stage";
    }
    void SetMainUnit()
    {
        for(int i = 0;i< mainUnitTr.Length; i++)
        {
            if(player.mainUnitList[i]>=0)
                player.GetUnitObject(player.mainUnitList[i]).transform.position= mainUnitTr[i].position;
        }      
    }
    void SetWaitUnit()
    {
        for (int i = 0; i < player.unitCount; i++)
        {
            player.GetUnitObject(i).transform.position = wp.GetPoint(i);
        }
    }
    public void ChangeState(STATE state)
    {
        switch (state)
        {
            case STATE.MAIN:
                SetMainUnit();
                break;
            case STATE.UNITMANAGEMENT:
                SetWaitUnit();
                break;
            case STATE.SELECTBATTLE:
                break;
            case STATE.BATTLE:
                break;
        }
    }
}
