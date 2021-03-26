using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
[RequireComponent(typeof(UnitMover))]
public class PatrollingState : State
{
    [SerializeField] bool _warning;
    [SerializeField] Transform[] _path;

    private Vector3[] _baseWarningPath = new Vector3[]{
                                            new Vector3 (2,0,2),
                                            new Vector3 (-2,0,-2),
                                            new Vector3 (-2,0,2),
                                            new Vector3 (2,0,-2)};
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
    }

    private void OnDisable()
    {
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

    private Vector3[] ConvertPath(Transform[] path)
    {
        var convertedPath = new Vector3[path.Length];
        for (var i = 0; i < path.Length; i++)
            convertedPath[i] = path[i].position;
        return convertedPath;
    }

    private Vector3[] CreatePath()
    {
        if (_path.Length == 0)
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
            return ConvertPath(_path);
        }
    }
}
