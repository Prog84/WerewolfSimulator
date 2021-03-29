using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class Targeter : MonoBehaviour
{
    [SerializeField] private float _range;
    [SerializeField] [Range(0, 360)] private float _angle;
    [SerializeField] private Transform _enemiesBox;
    [SerializeField] private LayerMask _obstacleMask;

    private List<Transform> _enemiesList;
    private Transform _player;
    private List<Transform> _targets;
    private Transform _currentTarget;

    public event UnityAction<Transform> TargetChanged;

    private void Awake()
    {
        _player = GetComponent<Transform>();
        _targets = new List<Transform>();
        _enemiesList = new List<Transform>();
    }

    private void OnEnable()
    {
        FillEnemiesList(_enemiesBox);
    }

    private void OnDisable()
    {
        _currentTarget = null;
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
        var distance = Vector3.Distance(enemy.position, _player.position);
        if (distance <= _range)
        {
            var direction = (enemy.position - _player.position).normalized;
            var angle = Vector3.Angle(_player.forward, direction);
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
                _targets.Add(enemy);
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
            if (Vector3.Distance(_player.position, _targets[i].position) < Vector3.Distance(_player.position, _targets[minIndex].position))
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

