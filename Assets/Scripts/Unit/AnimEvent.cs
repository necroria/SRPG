using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour {
    public Unit unit;
	void Attack()
    {
        unit.Attack();
    }
}
