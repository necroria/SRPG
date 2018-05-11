using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPlaceCamera : MonoBehaviour {
    Vector3 basePos;
    Transform tr;
    public int unitCount;
    public float between;
    float x;
    //float y;
    public float sens = 0.3f;
    // Use this for initialization
    private void OnEnable()
    {
        try
        {
            tr.localPosition = basePos;
        }
        catch(System.NullReferenceException)
        {
            
        }
    }
    void Start () {
        tr = GetComponent<Transform>();
        basePos = tr.localPosition;
#if UNITY_EDITOR
        sens = 0.5f;
#elif UNITY_ANDROID
        sens = 0.005f;
#endif
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButton(0) || (Input.touchCount > 0))
        {
            if (SkillPanel.instance.gameObject.activeSelf == true)
                return;
#if UNITY_EDITOR

            //y = Input.GetAxis("Mouse Y");
            x = Input.GetAxis("Mouse X");

#elif UNITY_ANDROID
                

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                //y = touch.deltaPosition.y*sens ;
                x = touch.deltaPosition.x*sens ;
            }
#endif

            tr.localPosition = Mathf.Clamp(tr.localPosition.x+x,-(unitCount - 1)*between,0)*Vector3.right+basePos;

        }
    }
}
