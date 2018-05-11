using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectBattlePanel : MyPanel {
    public int currentChapter=1;
    public int currentSeason=1;
    public int currentDifficulty=0;
    public List<List<GameObject>> chapterListInSeason = new List<List<GameObject>>();
    public int selectStage = 0;
    public StageInfoPanel siP;
    public StageGuide sg;
    public StageButtonMoving sbm;
    public GameObject[] arrows;
    public Button[] difficulties;
	// Use this for initialization
	void Start () {
        sbm.Init();
        sg.Init(10,this);
        sg.SetGuideButton(chapterListInSeason[currentSeason-1].Count);
        ChangeCurrentChapter(currentChapter);
        arrows[0].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=> ChangeCurrentChapter(currentChapter-1));
        arrows[1].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ChangeCurrentChapter(currentChapter + 1));
        
        difficulties[currentDifficulty].interactable = false;
    }
	
	public void ChangeCurrentChapter(int chapter)
    {
        chapterListInSeason[currentSeason-1][currentChapter-1].SetActive(false);
        currentChapter = chapter;
        sg.ChangeCurrentGuideButton(chapter);
        chapterListInSeason[currentSeason - 1][chapter-1].SetActive(true);        
        arrows[0].SetActive(chapter != 1);
        switch (currentSeason)
        {
            case 1:
                arrows[1].SetActive(chapter != chapterListInSeason[currentSeason - 1].Count);
                break;
            case 2:
                break;
        }
        
    }
    public new void SetActive(bool value)
    {
        gameObject.SetActive(value);
        if (value)
            ChangeCurrentChapter(currentChapter);
    }
    public void ClickDifficult(int value)
    {
        difficulties[currentDifficulty].interactable = true;
        currentDifficulty = value;
        difficulties[currentDifficulty].interactable = false;
    }
    public void ClickBattleStart()
    {
        MainManager.Instance.LoadBattleScene(1,GetMapCode());
    }
    public void ClickStage(int i)
    {
        selectStage = i;
        siP.SetInfo(StageManager.Instance.GetStage(currentSeason,currentChapter,i));
        MainUiManager.Instance.ViewPanel(MainUiManager.PANEL.STAGEINFO);
    }
    string GetMapCode()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(1);
        sb.Append(currentSeason.ToString("D2"));
        sb.Append(currentChapter.ToString("D2"));
        sb.Append(currentDifficulty.ToString("D2"));
        sb.Append(selectStage.ToString("D2"));
        return sb.ToString();
    }
    private void OnGUI()
    {
        GUI.TextField(new Rect(0, 0, 100, 100), selectStage.ToString());
    }
}
