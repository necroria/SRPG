using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MyPanel {

	public void ClickUnitManagement()
    {
        MainUiManager.Instance.ViewPanel(MainUiManager.PANEL.UNITMANAGEMENT);
    }
    public void ClickSelectBattle()
    {
        MainUiManager.Instance.ViewPanel(MainUiManager.PANEL.SELECTBATTLE);
    }
}
