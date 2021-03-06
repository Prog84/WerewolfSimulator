using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UnitMover))]
public class SoldierAnimator : MonoBehaviour
{

    [SerializeField] private float _minWalkSpeed;

    private Animator _animator;
    private UnitMover _mover;

    public void Fall()
    {
        _animator.SetTrigger("Fall");
    }

    public void Damage()
    {
        _animator.SetTrigger("TakeDamage");
    }

    public void Grabbed()
    {
        _animator.SetTrigger("Grabbed");
    }

    public void Shoot()
    {
        _animator.SetTrigger("Shoot");
    }

    public void CheckGround()
    {
        _animator.SetTrigger("CheckGround");
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<UnitMover>();
    }

    private void FixedUpdate()
    {
        if (_mover.IsWalking)
            _animator.SetFloat("Speed", 1);
        else
            _animator.SetFloat("Speed", 0);
    }

    public bool GetAiming()
    {
        return _animator.GetBool("Aiming");
    }

    public void SetAiming(bool value)
    {
        if (_animator != null)
            _animator.SetBool("Aiming", value);
    }
}
