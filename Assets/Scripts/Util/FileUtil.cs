using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class FileUtil : MonoBehaviour {

    public static string dataPath(int stageNum)
    {
        return Path.Combine(Application.streamingAssetsPath, "Data/map_data_" + stageNum + ".json");
        //return Application.streamingAssetsPath + "/Data/map_data_" + stageNum + ".json";
    }
    //참고용
    IEnumerator GoodCase()
    {
        WWW web = new WWW("www.a.com/a.txt");
        do
        {
            yield return null;
        }
        while (!web.isDone);

        if (web.error != null)
        {
            Debug.LogError("web.error=" + web.error);
            yield break;
        }

        // 여기까지 오면 성공이다.  
        //  
    }
    public static JsonData LoadMapData(int stageNum)
    {
        string path = dataPath(stageNum);
        //string path = Application.dataPath + "/Resources/Data/map_data_" + stageNum + ".json";
        //string path = "Data/map_data_" + stageNum + ".json";
        //Debug.Log(path);
#if UNITY_ANDROID
        WWW file = new WWW(path);
        
        if (file!=null)
        {
            while (!file.isDone) ;
            string jsonStr = file.text;
            JsonData json = JsonMapper.ToObject(jsonStr);
            return json;
        }
#else
        Debug.Log(File.Exists(path));

        if (File.Exists(path)){
            string jsonStr = File.ReadAllText(path);
            JsonData json = JsonMapper.ToObject(jsonStr);
            return json;
        }
#endif

        //Object obj = Resources.Load(path) ;
        //string path = Application.dataPath + "/Resources/Data/map_data_" + stageNum + ".json";
        //Debug.Log(obj);
        //if (obj)
        //{
        //    string jsonStr = obj.ToString();
        //    JsonData json = JsonMapper.ToObject(jsonStr);
        //    return json;
        //}

        return null;
    }
}
