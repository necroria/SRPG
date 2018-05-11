using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using System.Text;

public class FileUtil : MonoBehaviour {

    public static string dataPath(string mapCode)
    {
        return Application.streamingAssetsPath+"/Data/map_data_" + mapCode + ".json";
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
    public static List<JsonData> LoadJsonDataInFolder(string folderName)
    {
        
        string path = Path.Combine(Application.streamingAssetsPath, "Data/" + folderName);
        List<JsonData> jsonDatas = new List<JsonData>();
        FileInfo[] info = new DirectoryInfo(path).GetFiles();

        for (int i = 0; i < info.Length; i++)
        {
            if (info[i].Name.Contains(".meta"))
            {
                continue;
            }
            JsonData jd = LoadJData(path + "/" + info[i].Name);
            if (jd != null)
            {
                jsonDatas.Add(jd);
            }
            
        }
        if (jsonDatas.Count > 0)
        {
            return jsonDatas;
        }
        else
        {
            return null;
        }
        
    }
    public static JsonData LoadJsonData(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Data/"+fileName+".json");

        return LoadJData(path);
    }
    public static JsonData LoadPlayerData()
    {
        string path = Application.streamingAssetsPath+ "/Data/player.json";

        return LoadJData(path);
    }
    public static JsonData LoadMapData(string mapCode)
    {
        string path = dataPath(mapCode);

        return LoadJData(path);
    }
    static JsonData LoadJData(string path)
    {
#if UNITY_ANDROID
        WWW file = new WWW(path);

        if (file != null)
        {
            while (!file.isDone) ;
            string jsonStr = Encoding.UTF8.GetString(file.bytes);

            Debug.Log(jsonStr);
            JsonData json = JsonMapper.ToObject(jsonStr.Trim());
            return json;
        }
   
#else
        Debug.Log(File.Exists(path));
        if (File.Exists(path))
        {
            string jsonStr = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            JsonData json = JsonMapper.ToObject(jsonStr);
            return json;
        }
        //if (File.Exists(path)){
        //    string jsonStr = File.ReadAllText(path);
        //    JsonData json = JsonMapper.ToObject(jsonStr);
        //    return json;
        }
#endif
        return null;
    }
}
