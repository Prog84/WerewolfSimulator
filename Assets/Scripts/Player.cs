using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Targeter))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _MaxHP;

    private float _HP;
    private Rigidbody _body;
    private CapsuleCollider _collider;
    private PlayerMover _mover;
    private Targeter _targeter;

    public bool InMonsterForm => true;
    public bool IsAlive => _HP > 0;

    public event UnityAction Died;

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
    }

    private void Die()
    {
        _targeter.enabled = false;
        _body.useGravity = false;
        _collider.enabled = false;
        _mover.enabled = false;
        Died?.Invoke();
    }
}
