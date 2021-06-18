using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private Vector3 _targetPosition;
    private bool _isRotate = false;

    public event UnityAction DieWolf;

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
            _isRotate = _mover.RotateTo(_soldier.Target.transform.position);
        }
    }

    private void OnDisable()
    {
        _soldier.SetAiming(false);
    }

    private void Update()
    {
        if (_targetBody == null)
            _targetPosition = _soldier.Target.transform.position;
        else
        {
            _targetPosition = _targetBody.bounds.center;
            //StartCoroutine(WaitRotation());
        }
        _isRotate = false;
        _isRotate = _mover.RotateTo(_targetPosition);
        if (_lastAttackTime <= 0 && _isRotate == true)
        {
            Attack(_targetPosition);
            _lastAttackTime = _delay;
        }

        _lastAttackTime -= Time.deltaTime;
    }

    private void Attack(Vector3 target)
    {
        _soldier.Shoot(target);
        DieWolf?.Invoke();
    }

    /*private IEnumerator WaitRotation()
    {
        _isRotate = false;
        _mover.RotateTo(_targetPosition);
        yield return new WaitForSeconds(1f);
        _isRotate = true;
    }*/
}
