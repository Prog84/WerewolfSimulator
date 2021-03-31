using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Targeter))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private Claw _clawRight;
    [SerializeField] private Claw _clawLeft;

    private PlayerMover _mover;
    private Targeter _targeter;
    private PlayerInput _input;
    private bool _isAttacking;
    private Coroutine _attacking;

    public bool IsAttacking 
    {
        get => _isAttacking && _attacking == null;

        private set
        {
            _isAttacking =  value;
            _targeter.enabled = !value;
            _input.IsON = !value;
        }
    }

    public event UnityAction Grabbed;
    public event UnityAction Attacked;

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();
        _targeter = GetComponent<Targeter>();
        _input = GetComponent<PlayerInput>();
        SetClaws(false, false);
    }

    private void SetClaws(bool right, bool left)
    {
        _clawRight.enabled = right;
        _clawLeft.enabled = left;
    }

    private void TryAttack(Transform target)
    {
        if (target == null)
            return;
        if (IsAttacking)
            return;
        if (Vector3.Distance(transform.position, target.position) < 1.7f)
            if (target.TryGetComponent(out Soldier soldier))
                if (soldier.IsAlive)
                    Attack(soldier);
    }

    private void Attack(Soldier target)
    {
        IsAttacking = true;
        if (_attacking != null)
            StopCoroutine(_attacking);
        _attacking = StartCoroutine(AttackA(target));
    }

    private IEnumerator AttackA(Soldier target)
    {
        //Time.timeScale = 0.3f;
        Attacked?.Invoke();
        //_mover.SetRotation(targetPosition, 0.2f);
        yield return new WaitForSeconds(0.05f);
        SetClaws(true, false);
        yield return new WaitForSeconds(0.38f);
        SetClaws(false, false);
        target.Hit(transform);
        yield return new WaitForSeconds(0.4f);
        IsAttacking = false;
        //Time.timeScale = 1.2f;
    }

    private void Update()
    {
        if (IsAttacking == false)
            TryAttack(_targeter.CurrentTarget);
    }

    private void Grab()
    {
        if (_targeter.CurrentTarget == null)
            return;
        Grabbed?.Invoke();
        SetClaws(true, true);
        var target = _targeter.CurrentTarget.GetComponent<Soldier>();
        _targeter.enabled = false;
        _input.TurnOff();
        var attackPosition = GetAttackPosition(target.transform, 2.5f);
        _mover.SetPosition(attackPosition, target.transform.position, 0.3f);
        target.GetGrabbed(attackPosition);
        _mover.StickToPosition(GetEnemyFallPosition(target.transform, 1.7f), 1f, 1f);
    }

    private Vector3 GetAttackPosition(Transform target, float attackDistance)
    {
        var path = target.position - transform.position;
        return transform.position + path.normalized * (path.magnitude - attackDistance);
    }

    private Vector3 GetEnemyFallPosition(Transform target, float falldistance)
    {
        var path = target.position - transform.position;
        return transform.position + path.normalized * (path.magnitude + falldistance);
    }
}
