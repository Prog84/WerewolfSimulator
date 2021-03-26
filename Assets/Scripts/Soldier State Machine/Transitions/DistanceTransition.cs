using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class DistanceTransition : Transition
{
    [SerializeField] private float _min;
    [SerializeField] private float _max;
    [SerializeField] private float _randomSpread;

    private Transform _target;

    private void Start()
    {
        _min += Random.Range(-_randomSpread, _randomSpread);
        _max += Random.Range(-_randomSpread, _randomSpread);
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