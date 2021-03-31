using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class Targeter : MonoBehaviour
{
    [SerializeField] private float _MaxRange;
    [SerializeField] [Range(0, 360)] private float _angle;
    [SerializeField] private Transform _enemiesBox;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField]private Transform _playerTargeter;

    private List<Transform> _enemiesList;
    private List<Transform> _targets;
    private Transform _currentTarget;
    private float _MinRange;

    public Transform CurrentTarget => _currentTarget;

    public event UnityAction<Transform> TargetChanged;

    public event UnityAction TurnedOFF; 

    private void Awake()
    {
        _targets = new List<Transform>();
        _enemiesList = new List<Transform>();
        _MinRange = Vector3.Distance(transform.position, _playerTargeter.position);
    }

    private void OnEnable()
    {
        FillEnemiesList(_enemiesBox);
    }

    private void OnDisable()
    {
        _currentTarget = null;
        TurnedOFF?.Invoke();
    }

    private void FillEnemiesList(Transform box)
    {
        _enemiesList.Clear();
        var enemies = box.GetComponentsInChildren<Soldier>();
        foreach (var enemy in enemies)
        {
            _enemiesList.Add(enemy.transform);
        }
    }

    private void Update()
    {
        UpdateTargets();
        UpdateCurrentTarget();
    }

    private bool CheckTargetable(Transform enemy)
    {
        var distance = Vector3.Distance(enemy.position, _playerTargeter.position);
        if (_MinRange <= distance && distance <= _MaxRange)
        {
            var direction = (enemy.position - _playerTargeter.position).normalized;
            var angle = Vector3.Angle(_playerTargeter.forward, direction);
            if (angle <= _angle/2)
            {
                if (!Physics.Raycast(transform.position, direction, distance, _obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void UpdateTargets()
    {
        _targets.Clear();
        foreach (var enemy in _enemiesList)
        {
            if (CheckTargetable(enemy))
            {
                if (enemy.TryGetComponent(out Soldier soldier))
                {
                    if (soldier.IsAlive)
                        _targets.Add(enemy);
                }
            }
        }
    }

    private Transform GetNearesTarget()
    {
        if (_targets.Count == 0)
            return null;
        int minIndex = 0;
        for (var i = 0; i< _targets.Count; i++)
        {
            if (Vector3.Distance(_playerTargeter.position, _targets[i].position) < Vector3.Distance(_playerTargeter.position, _targets[minIndex].position))
            {
                minIndex = i;
            }
        }
        return _targets[minIndex];
    }

    private void UpdateCurrentTarget()
    {
        var newTarget = GetNearesTarget();
        if (newTarget != _currentTarget)
        {
            TargetChanged?.Invoke(newTarget);
            _currentTarget = newTarget;
        }
    }

}

