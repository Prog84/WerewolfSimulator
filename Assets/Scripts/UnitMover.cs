using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    public float RotationBoost;

    private Vector3 _targetPosition;
    private Quaternion _targetRotation;
    private float _distanseCheck = 0.3f;
    private float _rotationCheck = 5f;
    private Coroutine _patrolling;

    public bool IsMoving => _positionReached == false || _rotationReached == false;
    public bool IsWalking => _positionReached == false;
    private bool _positionReached => Vector3.Distance(transform.position, _targetPosition) < _distanseCheck;
    private bool _rotationReached => Quaternion.Angle(transform.rotation, _targetRotation) < _rotationCheck;

    private void Awake()
    {
        _targetPosition = transform.position;
        _targetRotation = transform.rotation;
        RotationBoost = 1;
    }

    public void AimTo(Quaternion targetRotation)
    {
        transform.rotation = (Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed * RotationBoost));
    }

    public void MoveTo(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        RotateTo(targetPosition);
    }
    public void RotateTo(Vector3 targetPosition)
    {
        var direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        _targetRotation = Quaternion.LookRotation(direction);
    }

    public void Stop()
    {
        if (_patrolling != null)
            StopCoroutine(_patrolling);
        _targetPosition = transform.position;
        _targetRotation = transform.rotation;
    }
    public void Patrol(Vector3[] path)
    {
        if (_patrolling != null)
            StopCoroutine(_patrolling);
        _patrolling = StartCoroutine(Patrolling(path));
    }

    private IEnumerator Patrolling(Vector3[] path)
    {
        while (path.Length > 0)
        {
            for (var i = 0; i<path.Length; i++)
            {
                MoveTo(path[i]);
                while (IsMoving)
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }

    private void Update()
    {
        if (_positionReached == false)
        {
            StepTo(_targetPosition);
        }
        if (_rotationReached == false)
        {
            AimTo(_targetRotation);
        }
    }
    private void StepTo(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        transform.position = Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime * _moveSpeed);
    }

}
