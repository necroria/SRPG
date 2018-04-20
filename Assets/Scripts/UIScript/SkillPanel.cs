using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillPanel : MonoBehaviour {
    private static SkillPanel _instance = null;
    public static SkillPanel instance
    {
        get { return _instance; }
    }
    
    public SkillListPanel listPanel;
    public SkillInfoPanel infoPanel;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        gameObject.SetActive(false);
        if (_instance == null)
            _instance = this;
    }
    private void Start()
    {
        RectTransform rtr = GetComponent<RectTransform>();
        listPanel.gameObject.GetComponent<RectTransform>().sizeDelta=new Vector2(rtr.rect.size.x / 3 * 2,0);
        infoPanel.gameObject.GetComponent< RectTransform>().sizeDelta = new Vector2(rtr.rect.size.x / 3, 0);
    }
}
