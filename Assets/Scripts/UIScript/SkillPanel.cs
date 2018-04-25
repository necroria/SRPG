using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillPanel : MonoBehaviour {
    private static SkillPanel _instance = null;
    
    public static SkillPanel instance    
    {
        get {
            //Debug.Log(_instance);
            if (_instance == null)
            {

                _instance = GameObject.Find("CommonCanvas").transform.Find("SkillPanel").GetComponent<SkillPanel>();
            }
            return _instance;
        }
    }
    
    public SkillListPanel listPanel;
    public SkillInfoPanel infoPanel;
    public GameObject skillActButton;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        

        
    }
    private void Start()
    {
        RectTransform rtr = GetComponent<RectTransform>();
        
        RectTransform listRtr = listPanel.gameObject.GetComponent<RectTransform>();
        RectTransform infoRtr = infoPanel.gameObject.GetComponent<RectTransform>();
        
        listRtr.sizeDelta = new Vector2((rtr.rect.size.x-10) / 3 * 2, 0);
        infoRtr.sizeDelta = new Vector2((rtr.rect.size.x-10) / 3, 0);

        gameObject.SetActive(false);
    }
    public void OnSkillPanel(Unit unit, bool isBattle=false)
    {
        gameObject.SetActive(true);
        listPanel.AddSkillIcon(unit);
        infoPanel.Clear();
        skillActButton.SetActive(isBattle);
        
    }
    public void OffSkillPanel()
    {
        gameObject.SetActive(false);
    }
}
