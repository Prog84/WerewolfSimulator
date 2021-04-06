using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerSpeedBooster))]
[RequireComponent(typeof(PlayerAttacker))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _minSpeed;
    [SerializeField] private PlayerInput _input;

    private Rigidbody _body;
    private Coroutine _translating;
    private float _translatingSpeed;
    private PlayerSpeedBooster _booster;
    private PlayerAttacker _attacker;

    public float CurrentBoost => _booster.CurrentBoost;
    public float CurrentSpeed => MovingPower * _moveSpeed;
    public float MovingPower
    {
        get
        {
            if (_attacker.IsAttacking == false)
                return _inputVector.magnitude + _booster.CurrentBoost;
            else
                return 1 + _booster.CurrentBoost;
        }
    }
    public bool IsMoving => MovingPower > _minSpeed;
    private Vector3 _inputVector => new Vector3(_input.Horizontal, 0, _input.Vertical);

    public void SetPosition(Vector3 target, Vector3 lookTarget, float time)
    {
        if (_translating != null)
        {
            StopCoroutine(_translating);
            _translatingSpeed = 0;
        }
        var targetRotation = Quaternion.LookRotation((lookTarget - target).normalized);
        _translating = StartCoroutine(Translate(target, targetRotation, time));
    } 

    public void SetRotation(Vector3 target, float time)
    {
        StartCoroutine(Rotate(Quaternion.LookRotation(target - transform.position), time));
    }

    private IEnumerator Rotate(Quaternion rotation, float time)
    {
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            _body.MoveRotation(Quaternion.Slerp(_body.rotation, rotation, timer/time));
            yield return null;
        }
    }

    private IEnumerator Translate(Vector3 position, Quaternion rotation, float time)
    {
        _translatingSpeed = Vector3.Distance(position, transform.position) / time / _moveSpeed;
        float timer = 0;
        while (timer <= time)
        {
            transform.position = Vector3.Lerp(transform.position, position, timer / time);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        _translatingSpeed = 0;
    }

    public void StickToPosition(Vector3 target, float time, float startDelay)
    {
        StartCoroutine(StickTo(target, time, startDelay));
    }

    private IEnumerator StickTo(Vector3 target, float time, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        float timer = 0;
        while (timer <= time)
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.1f);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _booster = GetComponent<PlayerSpeedBooster>();
        _attacker = GetComponent<PlayerAttacker>();
    }

    void FixedUpdate()
    {
        if (IsMoving && _translating == null)
        {
            var direction = new Vector3(_input.Horizontal, 0, _input.Vertical);
            if (direction.magnitude > _minSpeed && _input.IsON)
                RotateTo(direction);
            else if (_attacker.IsAttacking)
                RotateTo(_attacker.CurrentTarget);
            Move();

        }
    }

    private void Move()
    {
        Vector3 direction;
        float speed;
        if (_attacker.IsAttacking)
        {
            direction = (transform.forward).normalized;
            speed = _attacker.MoveSpeed;
        }
        else
        {
            direction = (transform.forward + _inputVector).normalized;
            speed = CurrentSpeed;
        }
        _body.MovePosition(Vector3.Lerp(_body.position, _body.position + direction, Time.fixedDeltaTime * speed));
    }

    private void RotateTo(Vector3 direction)
    {
        _body.MoveRotation(Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * _rotateSpeed * _booster.CurrentRotation));
    }

    private void RotateTo(Transform target)
    {
        _body.MoveRotation(Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(target.position - transform.position), Time.fixedDeltaTime * 10));
    }
}
