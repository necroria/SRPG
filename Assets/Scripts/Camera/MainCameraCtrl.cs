using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraCtrl : MonoBehaviour
{
    BattleManager battleManager;
    Transform tr;
    // Use this for initialization
    public void Init(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }
    float x;
    float y;
    public float sens = 0.3f;
    private void Start()
    {
        tr = GetComponent<Transform>();
#if UNITY_EDITOR
        sens = 1f;
#elif UNITY_ANDROID
        sens = 0.01f;
#endif
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) || (Input.touchCount > 0))
        {
            if (SkillPanel.instance.gameObject.activeSelf == true)
                return;
#if UNITY_EDITOR

            y = Input.GetAxis("Mouse Y");
            x = Input.GetAxis("Mouse X");

#elif UNITY_ANDROID
                

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                y = touch.deltaPosition.y*sens ;
                x = touch.deltaPosition.x*sens ;
            }
#endif

            tr.position = Mathf.Clamp(tr.position.x - y, -battleManager.mapYSize / 2 - 5f + 3f, battleManager.mapYSize / 2 - 5f - 0f) * Vector3.right + Mathf.Clamp(tr.position.z + x, -battleManager.mapXSize / 2 + 2f, battleManager.mapXSize / 2 - 2f) * Vector3.forward + tr.position.y * Vector3.up;

        }
    }
    private void OnGUI()
    {
        //GUI.Box(new Rect(0f, 0f, 100f, 30f), x + " " + y);
        //sens = float.Parse(GUI.TextField(new Rect(0f, 30f, 100, 30f), sens.ToString()));
    }
}
