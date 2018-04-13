using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilBtnPanel : MyPanel {

	
    public void TurnEnd()
    {
        BattleManager.instance.TurnEnd();
    }
}
