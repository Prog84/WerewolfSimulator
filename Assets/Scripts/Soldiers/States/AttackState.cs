using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Soldier))]
[RequireComponent(typeof(UnitMover))]
public class AttackState : State
{
    [SerializeField] private int _damage;

    private float _delay;
    private float _lastAttackTime;
    private Soldier _soldier;
    private UnitMover _mover;
    private CapsuleCollider _targetBody;

    private void Awake()
    {
        _soldier = GetComponent<Soldier>();
        _mover = GetComponent<UnitMover>();
        _delay = _soldier.AttackTime;
    }

    private void OnEnable()
    {
        _soldier.SetLineofSightColor(2);
        _soldier.SetAiming(true);
        if (Target != null)
        {
            if (Target.TryGetComponent(out CapsuleCollider body))
                _targetBody = body;
        }
    }

    private void OnDisable()
    {
        _soldier.SetAiming(false);
    }

    private void Update()
    {
        Vector3 targetPosition;
        if (_targetBody == null)
            targetPosition = _soldier.Target.transform.position;
        else
            targetPosition = _targetBody.bounds.center;
        _mover.RotateTo(targetPosition);
        if (_lastAttackTime <= 0)
        {
            Attack(targetPosition);
            _lastAttackTime = _delay;
        }

        _lastAttackTime -= Time.deltaTime;
    }

    private void Attack(Vector3 target)
    {
        _soldier.Shoot(target);
    }
}
