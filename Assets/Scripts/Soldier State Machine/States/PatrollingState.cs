using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
[RequireComponent(typeof(UnitMover))]
public class PatrollingState : State
{
    [SerializeField] bool _warning;
    [SerializeField] Transform _pathParent;

    private Vector3[] _baseWarningPath = new Vector3[]{
                                            new Vector3 (0.1f,0,0.1f),
                                            new Vector3 (-0.1f,0,-0.1f),
                                            new Vector3 (-0.1f,0,0.1f),
                                            new Vector3 (0.1f,0,-0.1f)};
    private Soldier _soldier;
    private UnitMover _mover;

    private void Awake()
    {
        _soldier = GetComponent<Soldier>();
        _mover = GetComponent<UnitMover>();
    }

    private void OnEnable()
    {
        if (_warning)
        {
            _soldier.SetLineofSightColor(1);
            _soldier.SetAiming(true);

        }
        else
        {
            _soldier.SetLineofSightColor(0);
            _soldier.SetAiming(false);
        }
        _mover.Patrol(CreatePath());
        _mover.RotationBoost = 0.1f;
    }

    private void OnDisable()
    {
        _mover.RotationBoost = 1;
        _mover.Stop();
    }

    private Vector3[] CreateBasePath(Vector3 position)
    {
        position.y = 0;
        var path = new Vector3[_baseWarningPath.Length];
        for(var i=0; i < path.Length; i++)
        {
            path[i] = _baseWarningPath[i] + position;
        }
        return path;
    }

    private Vector3[] ConvertPath(Transform pathParent)
    {
        var path = pathParent.GetComponentsInChildren<Transform>();
        var convertedPath = new Vector3[path.Length-1];
        for (var i = 0; i < path.Length-1; i++)
        {
            convertedPath[i] = path[i+1].position;
        }
        return convertedPath;
    }

    private Vector3[] CreatePath()
    {
        if (_pathParent == null)
        {
            Vector3 pathCenter;
            if (_soldier.LastSeenTargetPosition != Vector3.zero)
                pathCenter = _soldier.LastSeenTargetPosition;
            else
                pathCenter = _soldier.transform.position;
            return CreateBasePath(pathCenter);
        }
        else
        {
            var path = ConvertPath(_pathParent);
            List<float> distanses = new List<float>();
            var nearestPointIndex = 0;
            for (var i = 0; i < path.Length; i++)
            {
                distanses.Add(Vector3.Distance(transform.position, path[i]));
                if (distanses[nearestPointIndex] > distanses[i])
                    nearestPointIndex = i;
            }
            var sortedPath = new Vector3[path.Length];
            for (var i = 0; i < path.Length; i++)
            {
                sortedPath[i] = path[(i + nearestPointIndex) % path.Length];
            }
            return sortedPath;
        }
    }
}