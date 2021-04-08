using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class KillTranzition : Transition
{
    private Player _target;

    private void OnEnable()
    {
        _target = null;
    }

    private void OnDisable()
    {
        if (_target != null)
            _target.Died -= OnTargetKill;
    }

    private void OnTargetKill()
    {
        NeedTransit = true;
    }

    private void Update()
    {
        if (_target == null)
        {
            TryUpdateTarget();
        }
            
    }

    private void TryUpdateTarget()
    {
        if (Target != null)
        {
            if (Target.TryGetComponent(out Player player))
            {
                _target = player;
                _target.Died += OnTargetKill;
            }
        }
    }
}
