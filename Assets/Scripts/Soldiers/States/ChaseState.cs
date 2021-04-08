using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
[RequireComponent(typeof(UnitMover))]
public class ChaseState : State
{
    private Soldier _soldier;
    private UnitMover _mover;

    private void Awake()
    {
        _soldier = GetComponent<Soldier>();
        _mover = GetComponent<UnitMover>();
    }

    private void OnEnable()
    {
        _soldier.SetLineofSightColor(2);
        _soldier.SetAiming(true);
    }

    private void OnDisable()
    {
        _mover.Stop();
    }

    private void Update()
    {
        _mover.MoveTo(_soldier.Target.transform.position);
    }
}
