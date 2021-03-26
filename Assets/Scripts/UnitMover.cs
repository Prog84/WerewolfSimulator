using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private Vector3 _targetPosition;
    private float _distanseCheck = 0.3f;
    private Coroutine _patrolling;
    //private Rigidbody rb;

    public bool IsMoving => _targetReached == false;
    private bool _targetReached => Vector3.Distance(transform.position, _targetPosition) < _distanseCheck;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        _targetPosition = transform.position;
    }

    public void StepTo(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        transform.rotation = (Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotationSpeed));
        transform.position = Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime * _moveSpeed);
    }

    public void AimAt(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        transform.rotation = (Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotationSpeed));
    }

    public void MoveTo(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void Stop()
    {
        if (_patrolling != null)
            StopCoroutine(_patrolling);
        _targetPosition = transform.position;
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
                while (_targetReached == false)
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }

    private void Update()
    {
        if (_targetReached == false)
        {
            StepTo(_targetPosition);
        }
    }

}
