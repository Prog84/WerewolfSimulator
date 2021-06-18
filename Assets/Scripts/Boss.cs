using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Boss : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ps;
    [SerializeField] private ParticleSystem _pLeftRainbow;
    [SerializeField] private ParticleSystem _pRightRainbow;
    [SerializeField] private ParticleSystem _onHitBlood;
    [SerializeField] private AttackState _attackState;
    private int _countHit = 0;
    private int _hitToDie = 4;
    private StateMachine _state;
    private SoldierAnimator _animator;
    private List<Wolf> _wolves;

    private void Awake()
    {
        _wolves = new List<Wolf>();
        _animator = GetComponent<SoldierAnimator>();
        _state = GetComponent<StateMachine>();
        _attackState = GetComponent<AttackState>();
        _attackState.DieWolf += OnDieWolf;
    }

    public void AttackBoss()
    {
        if (_countHit == 0)
        {
            _wolves.AddRange(FindObjectsOfType<Wolf>());
        }
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        if (_countHit == _hitToDie)
        {
            _countHit++;
            _animator.Fall();
            Time.timeScale = 0.2f;
            _state.enabled = false;
            yield return new WaitForSeconds(1.5f);
            _ps.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
            _ps.Play();
            Time.timeScale = 1f;
            _pLeftRainbow.Play();
            _pRightRainbow.Play();
        } 
        else if (_countHit < _hitToDie)
        {
            _countHit++;
            _animator.Damage();
            Instantiate(_onHitBlood, transform.position + Vector3.up, Quaternion.LookRotation(transform.forward));
        }  
    }

    private void OnDieWolf()
    {
        if (_wolves.Count > 0)
        {
            var wolf = _wolves[Random.Range(0, _wolves.Count)];
            wolf.Die();
            _wolves.Remove(wolf);
        }  
    }
}
