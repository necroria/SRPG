using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainManager : MonoBehaviour {
    public static int playBattleNum = 1;
    public Text text;
    public Player player;
    public Transform[] mainUnitTr;
    public WaitPlace wp;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        player.Init();
        wp.Init(player.unitPrefabList.Count);
        SetWaitUnit();
        SetMainUnit();
        
    }
    public void LoadBattleScene()
    {
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
        for (int i = 0; i < player.unitPrefabList.Count; i++)
        {
            Debug.Log(i);
            player.GetUnitObject(i).transform.position = wp.GetPoint(i);
        }
    }
}
