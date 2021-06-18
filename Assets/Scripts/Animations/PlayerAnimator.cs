using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerAttacker))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private float _maxRunAnimationSpeedBoost;

    private Animator _animator;
    private PlayerMover _mover;
    private Player _player;
    private PlayerAttacker _attacker;
    private float _currentSpeed;
    private float _speedChangingSpeed = 3f;

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
        _player.Hit += OnHitFront;
    }

    private void OnDisable()
    {
        _player.Died -= OnDeath;
        _attacker.Grabbed -= OnGrab;
        _attacker.Attacked -= OnAttack;
        _player.Hit -= OnHitFront;
    }

    private void OnDeath()
    {
        _animator.SetTrigger("Die");
        _animator.SetBool("IsAlive", false);
        this.enabled = false;
    }

    private void OnGrab()
    {
        _animator.SetTrigger("Grab");
    }

    private void OnAttack(PlayerAttacker.AttackSpeed speed, bool isLeftSide)
    {
        _animator.SetBool("Side", isLeftSide);
        if (speed == PlayerAttacker.AttackSpeed.Slow)
            //_animator.SetTrigger("AttackSlow");
            _animator.SetTrigger("AttackFast");
        if (speed == PlayerAttacker.AttackSpeed.Fast)
            _animator.SetTrigger("AttackFast");
    }

    private void OnHitFront()
    {
        _animator.SetTrigger("HitFront");
    }

    private void LateUpdate()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _mover.CurrentSpeed, Time.deltaTime * _speedChangingSpeed);
        _animator.SetFloat("Speed", _currentSpeed);
        _animator.SetFloat("RunAnimationSpeed", 1 + _mover.CurrentBoost * _maxRunAnimationSpeedBoost);
    }



}
