using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4;
    [SerializeField] private float _rotateSpeed = 4;
    [SerializeField] private float _minSpeed = 0.2f;
    [SerializeField] private PlayerInput _input;

    private Rigidbody _body;
    private Coroutine _translating;
    private float _translatingSpeed;

    public float MovingSpeed
    {
        get
        {
            if (_translating == null)
                return _inputVector.magnitude;
            else
                return _translatingSpeed;
        }
    }
    public bool IsMoving => MovingSpeed > _minSpeed;
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

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (IsMoving && _translating == null)
        {
            var direction = new Vector3(_input.Horizontal, 0, _input.Vertical);
            MoveTo(direction);
            RotateTo(direction);
        }
    }

    private void MoveTo(Vector3 pos)
    {
        _body.MovePosition(Vector3.Lerp(_body.position, _body.position + pos, Time.fixedDeltaTime * _moveSpeed));
    }

    private void RotateTo(Vector3 position)
    {
        _body.MoveRotation(Quaternion.Slerp(_body.rotation, Quaternion.LookRotation(position), Time.fixedDeltaTime * _rotateSpeed));
    }

}
