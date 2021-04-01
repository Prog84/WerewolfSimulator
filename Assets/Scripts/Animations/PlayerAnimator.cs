using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerAttacker))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerMover _mover;
    private Player _player;
    private PlayerAttacker _attacker;
    private float _currentSpeed;
    private float _speedChangingSpeed = 2f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<PlayerMover>();
        _player = GetComponent<Player>();
        _attacker = GetComponent<PlayerAttacker>();
    }

    private void OnEnable()
    {
        _player.Died += OnDeath;
        _attacker.Grabbed += OnGrab;
        _attacker.Attacked += OnAttack;
        _attacker.AttackedRun += OnAttackRun;
    }

    private void OnDisable()
    {
        _player.Died -= OnDeath;
        _attacker.Grabbed -= OnGrab;
        _attacker.Attacked -= OnAttack;
        _attacker.AttackedRun -= OnAttackRun;
    }

    private void OnDeath()
    {
        _animator.SetTrigger("Die");
    }

    private void OnGrab()
    {
        _animator.SetTrigger("Grab");
    }

    private void OnAttack()
    {
        _animator.SetTrigger("Attack");
    }

    private void OnAttackRun()
    {
        _animator.SetTrigger("Attack2");
    }

    private void LateUpdate()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _mover.MovingSpeed, Time.deltaTime * _speedChangingSpeed);
        _animator.SetFloat("Speed", _currentSpeed);
    }



}
