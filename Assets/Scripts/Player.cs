using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Targeter))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _MaxHP;

    private float _HP;
    private Rigidbody _body;
    private CapsuleCollider _collider;
    private PlayerMover _mover;
    private Targeter _targeter;
    private PlayerInput _input;
    private Claw[] _claws;

    public bool InMonsterForm => true;
    public bool IsAlive => _HP > 0;

    public event UnityAction Died;
    public event UnityAction Attacked;

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError($"damage {damage} < 0 ");
            return;
        }
        _HP = Mathf.Clamp(_HP-damage, 0, _MaxHP);
        if (IsAlive == false)
            Die();
    }

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _mover = GetComponent<PlayerMover>();
        _targeter = GetComponent<Targeter>();
        _input = GetComponent<PlayerInput>();
        _claws = GetComponentsInChildren<Claw>();
        SetClaws(false);
        _HP = _MaxHP;
    }

    private void Die()
    {
        _targeter.enabled = false;
        _body.useGravity = false;
        _collider.enabled = false;
        _mover.enabled = false;
        Died?.Invoke();
    }

    public void Attack()
    {
        if (_targeter.CurrentTarget == null)
            return;
        Attacked?.Invoke();
        SetClaws(true);
        var target = _targeter.CurrentTarget.GetComponent<Soldier>();
        _targeter.enabled = false;
        _input.TurnOff();
        var attackPosition = GetAttackPosition(target.transform, 2.5f);
        _mover.SetPosition(attackPosition, target.transform.position, 0.3f);
        target.GetAttaked(attackPosition);
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

    private void SetClaws(bool state)
    {
        foreach (Claw claw in _claws)
            claw.enabled = state;
    }
}
