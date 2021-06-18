using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Wolf : MonoBehaviour
{
    private Boss _boss;
    public NavMeshAgent _agent;
    private Animator _animator;
    private Player _player;
    private bool _isBoss = false;
    private bool _coolDown = false;
    private bool _isDie = false;
    public Vector3 _target = Vector3.zero;
    private Enemies _enemies;
    private float _countEnemies;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        _agent.enabled = true;
        _animator.SetFloat("Speed", 1f);
        _animator.SetFloat("RunAnimationSpeed", 1f);
        _enemies = FindObjectOfType<Enemies>();
        _enemies.Died += OnSoldiersDied;
        _boss = FindObjectOfType<Boss>();
        _target = transform.position;
    }

    private void OnDisable()
    {
        _agent.enabled = false;
    }

    private void Update()
    {
        if (_isDie == false)
        {
            if (_isBoss)
            {
                if (_agent.enabled)
                {
                    _agent.SetDestination(_target);
                    _animator.SetFloat("Speed", 1.5f);
                    _animator.SetFloat("RunAnimationSpeed", 1.5f);
                }
                if (Vector3.Distance(transform.position, _target) < 3.5f)
                {
                    if (_coolDown == false)
                    {
                        StartCoroutine(Attack());
                    }

                }
            }
            else
            {
                if (Vector3.Distance(_boss.transform.position, _target) > 13.5f)
                {
                    SetTatget(_player.transform.position);
                }
                else
                {
                    _target = transform.position;
                }
            }
        }
    }

    public void SetTatget(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    public void SetBoss(Transform transform)
    {
        _target = transform.position;
        _isBoss = true;
        _agent.speed = 4.5f;
        _agent.stoppingDistance = 1.8f;
    }
    private IEnumerator Attack()
    {
        _coolDown = true;
        transform.rotation = Quaternion.LookRotation(_target - transform.position);
        yield return new WaitForSeconds(Random.Range(0.5f, 0.9f));
        _animator.SetFloat("RunAnimationSpeed", 0f);
        _animator.SetTrigger("AttackSlow");
        _animator.SetFloat("Speed", 0f);
        _coolDown = false;
    }

    public void Die()
    {
        StartCoroutine(DieWolf());
    }

    private IEnumerator DieWolf()
    {
        yield return new WaitForSeconds(Random.Range(5f, 5.5f));
        _animator.SetBool("IsAlive", false);
        _animator.SetTrigger("Die");
        _isDie = true;
    }

    private void OnSoldiersDied(float count)
    {
        if (count == 0)
        {
            _isBoss = false;
            _animator.SetFloat("Speed", 0f);
            _animator.SetFloat("RunAnimationSpeed", 0f);
            _agent.enabled = false;
        }
        _countEnemies = count;
    }
}
