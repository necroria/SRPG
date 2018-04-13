using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimCtrl : MonoBehaviour {
    public Animator anim;
    public Unit unit;
    
    public void Attack(Unit hitObjectUnit)
    {
        anim.SetTrigger("MainAttack");
        unit.attackUnit = hitObjectUnit;
    }
}
