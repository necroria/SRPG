using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageGuide : MonoBehaviour {
    public GameObject stageGuideBtnPrefab;
    GameObject[] stageGuideBtns;
    //RectTransform[] rectTransforms;
    Button[] buttons;
    SelectBattlePanel sbP;
    int currentBtn;
    // Use this for initialization
    public void Init (int maxChapter,SelectBattlePanel sbP) {
        this.sbP = sbP;
        stageGuideBtns = new GameObject[maxChapter];
        //rectTransforms = new RectTransform[maxChapter];
        buttons = new Button[maxChapter];
        for (int i=0;i< stageGuideBtns.Length; i++)
        {
            stageGuideBtns[i] = Instantiate<GameObject>(stageGuideBtnPrefab, transform);
            stageGuideBtns[i].name = "chapter" + (i + 1);
            //rectTransforms[i]=stageGuideBtns[i].GetComponent<RectTransform>();
            buttons[i] = stageGuideBtns[i].GetComponent<Button>();
            int chapter = i + 1;
            stageGuideBtns[i].GetComponent<Button>().onClick.AddListener(() =>  sbP.ChangeCurrentChapter(chapter) );
            
        }
        currentBtn = sbP.currentChapter;
    }
    public void ChangeCurrentGuideButton(int chapter)
    {

        buttons[currentBtn-1].interactable = true;
        buttons[chapter-1].interactable = false;
        currentBtn = chapter;
    }
	public void SetGuideButton(int count)
    {
        for(int i = 0; i <count; i++)
        {
            stageGuideBtns[i].SetActive(true);
        }
        for (int i = count; i < stageGuideBtns.Length; i++)
        {
            stageGuideBtns[i].SetActive(false);
        }
    }
    //public RectTransform GetGuideButtonRectTransform(int num)
    //{
    //    //챕터는 1부터 시작
    //    return rectTransforms[num-1];
    //}
	
}
