using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMover))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerMover _mover;
    private float _lastSpeed;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<PlayerMover>();
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
