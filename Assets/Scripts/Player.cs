using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Targeter))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _maxHP;
    [SerializeField] private float _maxRage;
    [SerializeField] private float _healthRegenTime;
    [SerializeField] private float _RageDecay;

    private float _HP;
    private float _rage;
    private CapsuleCollider _collider;
    private PlayerMover _mover;
    private Targeter _targeter;

    public bool InMonsterForm => true;
    public float MaxHP => _maxHP;
    public float MaxRage => _maxRage;
    public bool IsAlive => _HP > 0;

    public event UnityAction Died;
    public event UnityAction<float> HealthChanged;
    public event UnityAction<float> RageChanged;
    public event UnityAction Hit;

    public void HitFront()
    {
        _mover.enabled = false;
        Hit?.Invoke();
        StartCoroutine(StepBack(2.5f, 0.9f));
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError($"damage {damage} < 0 ");
            return;
        }
        _HP = Mathf.Clamp(_HP - damage, 0, _maxHP);
        HealthChanged?.Invoke(_HP);
        RiseRage(1);
        if (IsAlive == false)
            Die();
    }

    public void RiseRage(float amount)
    {
        if (amount < 0)
        {
            Debug.LogError($"amount {amount} < 0 ");
            return;
        }
        _rage = Mathf.Clamp(_rage + amount, 0, _maxRage);
        RageChanged?.Invoke(_rage);
    }

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _mover = GetComponent<PlayerMover>();
        _targeter = GetComponent<Targeter>();
        _HP = _maxHP;
        _rage = _maxRage/2;
    }

    private void Die()
    {
        _targeter.enabled = false;
        _mover.enabled = false;
        this.enabled = false;
        Died?.Invoke();
    }
    private void Start()
    {
        StartCoroutine(RegenHealth(0.1f));
        StartCoroutine(DecayRage());
    }

    private IEnumerator RegenHealth(float updateTime)
    {
        float time = Time.time;
        while (true)
        {
            if (Time.time - time >= _healthRegenTime)
            {
                _HP += 1;
                if (_HP > _maxHP)
                    _HP = _maxHP;
                HealthChanged?.Invoke(_HP);
                time = Time.time;
            }
            yield return new WaitForSeconds(updateTime);
        }
    }

    private IEnumerator DecayRage()
    {
        while (true)
        {
            _rage -= Mathf.Lerp(_RageDecay / 3, _RageDecay, _rage/_maxRage);
            if (_rage < 0)
                _rage = 0;
            RageChanged?.Invoke(_rage);
            yield return null;
        }
    }

    private IEnumerator StepBack(float distanse, float time)
    {
        var timer = 0f;
        var targetPos = transform.position - transform.forward * distanse;
        while (timer <= time)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10);
            timer += Time.deltaTime;
            yield return null;
        }
        _mover.enabled = true;
    }
}
