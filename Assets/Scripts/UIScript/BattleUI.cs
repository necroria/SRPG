using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour {
    public BtnPanel btnPanel;
    public UnitInfoPanel unitInfoPanel;
    public BattleInfoPanel battleInfoPanel;
    public UtilBtnPanel utilBtnPanel;
    public bool panelState = false;
    private void Start()
    {
        
        SetActivePanel(panelState);
    }
    public void SetActivePanel(bool value,Unit unit=null)
    {

        unitInfoPanel.SetActive(value);
        utilBtnPanel.SetActive(!value);
        btnPanel.SetActive(false);
        panelState = value;
        if (unit == null)
            return;
        if (unit.identify == Unit.IDENTIFY.ALLY)
        {
            btnPanel.SetActive(value);
        }
        unitInfoPanel.SetUnitInfo(unit);
    }
    public void Init(int maxTurn)
    {
        gameObject.SetActive(true);
        battleInfoPanel.maxTurn = maxTurn;
        battleInfoPanel.SetTurn(1);
    }
}
