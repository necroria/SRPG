using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageInfoPanel : MyPanel {

    public Text stageName;
    public Text stageInfo;

    public void SetInfo(Stage stage)
    {
        
        stageName.text = stage.name;
        stageInfo.text = stage.info;
    }
}
