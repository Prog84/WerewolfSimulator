using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class OutAttackRangeTranzition : Transition
{
    private Transform _target;
    private float _min;

    private void Start()
    {
        _min = GetComponent<Soldier>().AttackRange;
        _target = GetComponent<Soldier>().Target.transform;
    }

    private void Update()
    {
        var distanse = Vector3.Distance(transform.position, _target.position);
        if ((distanse > _min))
        {
            NeedTransit = true;
        }
    }
}
