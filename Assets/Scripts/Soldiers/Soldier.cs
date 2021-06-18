using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SoldierAnimator))]
public class Soldier : MonoBehaviour
{
    [SerializeField] private SoldierLineOfSight _lineOFSight;
    [SerializeField] private ParticleSystem _onHitBlood;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Weapon _weapon;

    private SoldierAnimator _animator;
    private Player _target;
    private Vector3 _lastSeenTargetPosition;
    private float _lastSeenTargetTime;
    private bool _isAlive = true;

    public float AttackRange => _weapon.Range;
    public float AttackTime => 1 / _weapon.RateofFire;
    public Player Target => _target;
    public bool IsAlive => _isAlive;
    public bool SeesTarget => _lineOFSight.VisibleTargets.Contains(_target.transform);

    public Vector3 LastSeenTargetPosition => _lastSeenTargetPosition;
    public float LastSeenTargetTime => _lastSeenTargetTime;

    public event UnityAction<Soldier> Died;


    public void Init(Player target)
    {
        _target = target;
    }

    public void Shoot(Vector3 target)
    {
        _weapon.Shoot(_shootPoint, target);
        _animator.Shoot();
    }

    public void SetLineofSightColor(int index)
    {
        _lineOFSight.SetMaterial(index);
    }
    public bool GetAiming()
    {
        return _animator.GetAiming();
    }

    public void SetAiming(bool value)
    {
        if (_animator != null)
            _animator.SetAiming(value);
    }

    private void Awake()
    {
        _animator = GetComponent<SoldierAnimator>();
        _target = FindObjectOfType<Player>();
        _lastSeenTargetPosition = transform.position;
        _lastSeenTargetTime = Time.time;
    }

    private void Update()
    {
        if (SeesTarget && _target != null)
        {
            _lastSeenTargetTime = Time.time;
            _lastSeenTargetPosition = _target.transform.position;
        }
    }

    private void Die()
    {
        if (TryGetComponent(out StateMachine machine))
            machine.enabled = false;
        if (TryGetComponent(out UnitMover mover))
            mover.enabled = false;
        if (TryGetComponent(out BoxCollider box))
            box.enabled = false;
        if (TryGetComponent(out Rigidbody body))
            Destroy(body);
        _lineOFSight.enabled = false;
        _isAlive = false;
        Died?.Invoke(this);
    }

    public void Hit(Vector3 attacker)
    {
        if (TryGetComponent(out Boss boss))
            return;
        Die();
        _animator.Fall();
        transform.rotation = Quaternion.LookRotation(attacker - transform.position);
        Instantiate(_onHitBlood, transform.position + Vector3.up/*+ new Vector3(0, attacker.y, 0)*/, Quaternion.LookRotation(transform.forward));
        //Instantiate(_onHitBlood, transform.position + Vector3.up, Quaternion.LookRotation(transform.forward));
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Wolf wolf))
        {
            Hit(wolf.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Wolf wolf))
        {
            Hit(wolf.transform.position);
        }
    }
}
