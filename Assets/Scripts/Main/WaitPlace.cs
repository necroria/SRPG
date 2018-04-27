using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPlace : MonoBehaviour {
    public Transform BaseTr;
    public List<Vector3> points = new List<Vector3>();
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void Init(int count)
    {
        
        for(int i = 0; i < count; i++)
        {
            //Instantiate<GameObject>(new GameObject(), Vector3.left * i, Quaternion.identity, BaseTr);
            points.Add(BaseTr.position + Vector3.left * i);
        }
    }
    public Vector3 GetPoint(int num)
    {
        return points[num];
    }
}
