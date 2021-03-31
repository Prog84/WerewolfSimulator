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
    private float _lastSpeed;

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
    }

    private void OnDisable()
    {
        _player.Died -= OnDeath;
        _attacker.Grabbed -= OnGrab;
        _attacker.Attacked -= OnAttack;
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

    private void Update()
    {
        UpdateMoveAnimation();
    }

    private void UpdateMoveAnimation()
    {
        float currentSpeed = 0;
        if (_mover.IsMoving)
            currentSpeed = _mover.MovingSpeed;
        currentSpeed += (_lastSpeed - currentSpeed) / 1.1f;
        _lastSpeed = currentSpeed;
        _animator.SetFloat("Speed", currentSpeed);
    }


}
