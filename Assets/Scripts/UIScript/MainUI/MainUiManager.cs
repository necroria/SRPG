using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUiManager : MonoBehaviour {
    private static MainUiManager _instance = null;
    public static MainUiManager Instance
    {
        get { return _instance; }
    }
    Stack<PANEL> panelStack = new Stack<PANEL>();
    public Camera[] cameras;
    public enum PANEL { NONE=-1,STATUS=0,MENU,UNITMANAGEMENT,SELECTBATTLE,STAGEINFO}
    public StatusPanel statusP;
    public MenuPanel MenuP;
    public UnitManagementPanel umP;
    public SelectBattlePanel sbP;
    public GameObject backBtn;
    public StageInfoPanel siP;
    PANEL currentPanel = PANEL.MENU;
    
    List<MyPanel> myPanels = new List<MyPanel>();
    private void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        myPanels.Add(statusP);
        myPanels.Add(MenuP);
        myPanels.Add(umP);
        myPanels.Add(sbP);
        myPanels.Add(siP);
        ViewPanel(PANEL.MENU);
    }
    public void ViewPanel(PANEL panel,bool isBack=false)
    {
        int panelNum = (int)panel;
        bool isChangeView = false;
        switch (panel)
        {
            case PANEL.NONE:
                break;
            case PANEL.STATUS:
                break;
            case PANEL.MENU:
                OnCamera(0);
                isChangeView = true;
                //panelStack.Clear(); 전투후 메인 씬으로 올지에 따라 결정
                //if(isBack)
                //panelStack.Push(PANEL.MENU);
                //currentPanel = PANEL.MENU;
                MainManager.Instance.ChangeState(MainManager.STATE.MAIN);
                break;
            case PANEL.UNITMANAGEMENT:
                OnCamera(1);
                isChangeView = true;
                MainManager.Instance.ChangeState(MainManager.STATE.UNITMANAGEMENT);
                break;
            case PANEL.SELECTBATTLE:
                isChangeView = true;
                MainManager.Instance.ChangeState(MainManager.STATE.SELECTBATTLE);
                break;
            case PANEL.STAGEINFO:
                
                break;

        }
        backBtn.SetActive(panel != PANEL.MENU);
        myPanels[panelNum].SetActive(true);
        if (!isBack)
        {
            panelStack.Push(currentPanel);
            currentPanel = panel;
        }
        if (!isChangeView)
            return;
        
        for (int i = 1; i < myPanels.Count; i++)
        {
            myPanels[i].SetActive(i == panelNum);
        }
    }
    /// <summary>
    /// main = 0, unit management = 1
    /// </summary>
    /// <param name="num"></param>
    void OnCamera(int num)
    {
        for (int i=0;i<cameras.Length;i++)
        {
            cameras[i].enabled = num==i;
        }
    }
    public void ClickBackButton()
    {
        myPanels[(int)currentPanel].SetActive(false);
        currentPanel=panelStack.Pop();
        ViewPanel(currentPanel,true);
        Debug.Log(panelStack.Count);
    }
}
