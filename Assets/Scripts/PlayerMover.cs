using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerSpeedBooster))]
[RequireComponent(typeof(PlayerAttacker))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _minSpeed;
    [SerializeField] private PlayerInput _input;

    private PlayerSpeedBooster _booster;
    private PlayerAttacker _attacker;

    public float CurrentBoost => _booster.CurrentBoost;
    public float CurrentMovingSpeed => CurrentSpeedModifier * _moveSpeed;
    public float CurrentSpeed => _inputVector.magnitude + CurrentBoost;
    public float CurrentSpeedModifier
    {
        get
        {
            if (_attacker.IsAttacking == false)
                return _inputVector.magnitude * _booster.CurrentSpeed;
            else
                return _booster.CurrentSpeed;
        }
    }
    public bool IsMoving => CurrentSpeedModifier > _minSpeed;
    private Vector3 _inputVector => new Vector3(_input.Horizontal, 0, _input.Vertical);

    private void Awake()
    {
        _booster = GetComponent<PlayerSpeedBooster>();
        _attacker = GetComponent<PlayerAttacker>();
    }

    private void Update()
    {
        if (IsMoving)
        {
            Move();
        }
        if (_attacker.IsAttacking)
        {
            RotateTo(_attacker.CurrentTarget);
        }
        else if (_inputVector.magnitude > _minSpeed && _input.IsON)
        {
            RotateTo(_inputVector);
        }
    }

    private void Move()
    {
        float speed;
        if (_attacker.IsAttacking)
        {
            speed = _attacker.AttackMovingSpeed;
        }
        else
        {
            speed = CurrentMovingSpeed;
        }
        transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
    }

    private void RotateTo(Vector3 direction)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotateSpeed * _booster.CurrentRotation);
    }

    private void RotateTo(Transform target)
    {
        var targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 400);
    }
}
