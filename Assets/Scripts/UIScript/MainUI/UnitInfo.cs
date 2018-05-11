using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitInfo : MonoBehaviour {
    public Text text;
    public Button button;
    public void Init(string name, bool usable)
    {
        text.text = name;
        button.gameObject.SetActive(!usable);
    }
	
}
