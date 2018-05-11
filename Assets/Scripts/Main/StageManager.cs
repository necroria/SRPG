using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class StageManager : MonoBehaviour {
    private static StageManager instance = null;
    public static StageManager Instance
    {
        get { return instance; }
    }
    [SerializeField]List<List<List<Stage>>> seasons = new List<List<List<Stage>>>();
    public int maxStageCount = 0;
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        List<JsonData> data = FileUtil.LoadJsonDataInFolder("Stage");
        //season loop
        for(int i = 0; i < data.Count; i++)
        {
            List<List<Stage>> chapters = new List<List<Stage>>();
            //chapter loop
            for (int j = 0; j < data[i].Count; j++)
            {
                List<Stage> stages = new List<Stage>();
                //stage loop
                if(maxStageCount< data[i][j].Count)
                {
                    maxStageCount = data[i][j].Count;
                }
                for(int k = 0; k < data[i][j].Count; k++)
                {
                    Stage stage = new Stage(i, j, k, data[i][j][k]["name"].ToString(), data[i][j][k]["info"].ToString());
                    stages.Add(stage);
                }
                chapters.Add(stages);
            }
            seasons.Add(chapters);
        }
    }	
    public Stage GetStage(int season, int chapter, int stage)
    {
        return seasons[season-1][chapter-1][stage-1];
    }
    public int ChapterCount(int season)
    {
        return seasons[season-1].Count;
    }
    public int SeasonCount()
    {
        return seasons.Count;
    }
}
