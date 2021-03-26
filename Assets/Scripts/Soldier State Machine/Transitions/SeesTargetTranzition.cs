using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class SeesTargetTranzition : Transition
{
    [SerializeField] private bool _condition;

    private Soldier _soldier;

    private void Awake()
    {
        _soldier = GetComponent<Soldier>();
    }

    void Update()
    {
        if (_soldier.SeesTarget == _condition)
        {
            NeedTransit = true;
        }
    }
}
