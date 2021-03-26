using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class IdleState : State
{
    private void OnEnable()
    {
        var soldier = GetComponent<Soldier>();
        soldier.SetLineofSightColor(0);
        soldier.SetAiming (false);
    }
}
