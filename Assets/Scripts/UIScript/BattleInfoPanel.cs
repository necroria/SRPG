using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleInfoPanel : MyPanel {
    public Text turnText;
    public int maxTurn;

    public void SetTurn(int i)
    {
        turnText.text = "남은 턴 수\n" + i + "/" + maxTurn;
    }
}
