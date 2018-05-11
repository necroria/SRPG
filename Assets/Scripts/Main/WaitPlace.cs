using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaitPlace : MonoBehaviour {
    public Transform BaseTr;
    public List<Vector3> points = new List<Vector3>();
    Player player;
    public GameObject UnitInfo;
    public Transform canvas;
    public float between = 1;
    public WaitPlaceCamera wpc;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void Init(int count,Player player)
    {
        this.player = player;
        for(int i = 0; i < count; i++)
        {
            //Instantiate<GameObject>(new GameObject(), Vector3.left * i, Quaternion.identity, BaseTr);
            points.Add(BaseTr.position + Vector3.left *i* between);
        }
        SetUnitName(count);
        wpc.unitCount = count;
        wpc.between = between;
    }
    public void SetUnitName(int count)
    {
        for(int i=0;i<count; i++)
        {

            UnitInfo info = Instantiate<GameObject>(UnitInfo,canvas).GetComponent<UnitInfo>();
            info.Init(player.GetUnit(i).name, player.GetUnitUsable(i));
            info.transform.localPosition = i* between * Vector3.right;
        }
        
    }
    public Vector3 GetPoint(int num)
    {
        return points[num];
    }
}
