using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class InAttackRangeTranzition : Transition
{
    private Transform _target;
    private float _min;
    private float _max;

    private void Start()
    {
        _min = 0;
        _max = GetComponent<Soldier>().AttackRange;
        _target = GetComponent<Soldier>().Target.transform;
    }

    private void Update()
    {
        var distanse = Vector3.Distance(transform.position, _target.position);
        if ((distanse > _min) && (distanse < _max))
        {
            NeedTransit = true;
        }
    }
}
