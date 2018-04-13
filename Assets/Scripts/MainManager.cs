using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainManager : MonoBehaviour {
    public static int playBattleNum = 1;
    public Text text;
    private void Awake()
    {
        DontDestroyOnLoad(this);
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
    private void Update()
    {
        
    }
}
