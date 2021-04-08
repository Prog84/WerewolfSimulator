using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(SoldierAnimator))]
public class SelebrateState : State
{
    private Soldier _soldier;
    private UnitMover _mover;
    private SoldierAnimator _animator;
    private Coroutine _cheking;

    private void Awake()
    {
        _soldier = GetComponent<Soldier>();
        _mover = GetComponent<UnitMover>();
        _animator = GetComponent<SoldierAnimator>();
    }

    private void OnEnable()
    {
        _soldier.SetLineofSightColor(0);
        _soldier.SetAiming(false);
        _cheking = StartCoroutine(CheckTarget(Target.transform.position));
    }

    private void OnDisable()
    {
        if (_cheking != null)
            StopCoroutine(_cheking);
    }

    private IEnumerator CheckTarget(Vector3 position)
    {
        if (_cheking != null)
            StopCoroutine(_cheking);
        _mover.MoveTo(position);
        while (_mover.IsMoving)
        {
            yield return new WaitForSeconds(1);
        }
        _animator.CheckGround();
    }
}
