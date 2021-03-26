using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class TimeTranzition : Transition
{
    [SerializeField] private float _time;

    private Soldier _soldier;

    private void Awake()
    {
        _soldier = GetComponent<Soldier>();
    }

    void Update()
    {
        if (Time.time > _time + _soldier.LastSeenTargetTime)
            NeedTransit = true;
    }
}
