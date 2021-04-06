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
    [SerializeField] private Attack[] _attacks;
    [SerializeField] private float _runAttackTime;

    private PlayerMover _mover;
    private Targeter _targeter;
    private PlayerInput _input;
    private bool _isAttacking = false;
    private float _attackSpeed;
    private Coroutine _attacking = null;
    private Transform _currentTarget;
    public bool IsAttacking 
    {
        get => _isAttacking;

        private set
        {
            _isAttacking =  value;
            _targeter.enabled = !value;
            _input.IsON = !value;
        }
    }
    public float MoveSpeed => _attackSpeed;
    public Transform CurrentTarget => _currentTarget;

    public event UnityAction Grabbed;
    public event UnityAction<AttackSpeed> Attacked;

    public enum AttackSpeed
    {
        Slow,
        Fast
    }

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
        foreach (Attack attack in _attacks)
        {
            if (CheckAttackAvalable(attack, target, out Soldier targetSoldier, out float distance))
            {
                DoAttack(targetSoldier, attack, Mathf.Clamp(distance - attack.Offset, 0, float.MaxValue) / attack.Time);
                return;
            }
        }
    }

    private void DoAttack(Soldier target, Attack attack, float speed)
    {
        _attackSpeed = speed;
        IsAttacking = true;
        _currentTarget = target.transform;
        if (_attacking != null)
            StopCoroutine(_attacking);
        _attacking = StartCoroutine(Attack(target, attack));
    }

    private IEnumerator Attack(Soldier target, Attack attack)
    {
        Time.timeScale = attack.SlowMotion;
        IsAttacking = true;
        Attacked?.Invoke(attack.AttackSpeed);
        yield return new WaitForSeconds(attack.PauseClawOn);
        SetClaws(attack.RightClaw, attack.LeftClaw);
        yield return new WaitForSeconds(attack.PauseClawOff);
        SetClaws(false, false);
        yield return new WaitForSeconds(attack.PauseTargetHit);
        target.Hit(transform);
        IsAttacking = false;
        _currentTarget = null;
        _attackSpeed = 0;
        Time.timeScale = 1f;
    }

    private bool CheckAttackAvalable(Attack attack, Transform target, out Soldier targetSoldier, out float distance)
    {
        targetSoldier = null;
        distance = 0;
        if (attack.MinSpeed <= _mover.MovingPower && _mover.MovingPower <= attack.MaxSpeed)
        {
            var maxRange = attack.Time * _mover.CurrentSpeed;
            distance = (Vector3.Distance(transform.position, target.position));
            if (distance <= maxRange)
            {
                if (target.TryGetComponent(out targetSoldier))
                {
                    if (targetSoldier.IsAlive)
                    {
                        if (attack.AttackSpeed == AttackSpeed.Fast)
                        {
                            if ((Time.time - _runAttackTime) < 1.5f)
                                return false;
                            _runAttackTime = Time.time;
                        }
                        return true;
                    }
                }
            }
        }
        return false;
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
