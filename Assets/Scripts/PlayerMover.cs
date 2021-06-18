using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerSpeedBooster))]
[RequireComponent(typeof(PlayerAttacker))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _minSpeed;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Enemies _enemies;

    private PlayerSpeedBooster _booster;
    private PlayerAttacker _attacker;
    private Boss _boss;
    private Animator _animator;
    private bool _coolDown = false;
    private bool _isBoss = false;
    private int _numberHitsBoss = 0;
    private Rigidbody _rigidbody;

    public float CurrentBoost => _booster.CurrentBoost;
    public float CurrentMovingSpeed => CurrentSpeedModifier * _moveSpeed;
    public float CurrentSpeed => _inputVector.magnitude + CurrentBoost;
    public float CurrentSpeedModifier
    {
        get
        {
            if (_attacker.IsAttacking == false)
                return (_inputVector.magnitude + _booster.CurrentSpeed - 1);
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
        _boss = FindObjectOfType<Boss>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_enemies._enemies.Count > 0 && _isBoss == false)
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

        if (Input.GetMouseButtonDown(1) && _numberHitsBoss < 5)
        {
            _rigidbody.isKinematic = true;
            if (_coolDown == false)
            {
                if (_numberHitsBoss < 5)
                {
                    _numberHitsBoss++;
                    StartCoroutine(Attack());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _animator.SetTrigger("AttackFast");
        }
    }

    private IEnumerator Attack()
    {
        _coolDown = true;
        _boss.AttackBoss();
        yield return new WaitForSeconds(0.1f);
        _animator.SetFloat("RunAnimationSpeed", 0f);
        _animator.SetTrigger("AttackSlow");
        yield return new WaitForSeconds(1f);
        _coolDown = false;
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
