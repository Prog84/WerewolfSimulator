using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SoldierAnimator))]
public class Soldier : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private SoldierLineOfSight _lineOFSight;

    private SoldierAnimator _animator;
    private Player _target;
    private Vector3 _lastSeenTargetPosition;
    private float _lastSeenTargetTime;

    public Player Target => _target;
    public bool SeesTarget => _lineOFSight.VisibleTargets.Contains(_target.transform);

    public Vector3 LastSeenTargetPosition => _lastSeenTargetPosition;
    public float LastSeenTargetTime => _lastSeenTargetTime;

    public event UnityAction<Soldier> Dying;


    public void Init(Player target)
    {
        _target = target;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Dying?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 target, GameObject bullet, Vector3 shootPoint)
    {
        _animator.Shoot();
        Vector3 direction = (target - shootPoint).normalized;
        Instantiate(bullet, shootPoint, Quaternion.LookRotation(direction));
    }

    public void SetLineofSightColor(int index)
    {
        _lineOFSight.SetMaterial(index);
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
        if (SeesTarget && _target !=null)
        {
            _lastSeenTargetTime = Time.time;
            _lastSeenTargetPosition = _target.transform.position;
        }
    }

    public bool GetAiming()
    {
        return _animator.GetAiming();
    }

    public void SetAiming(bool value)
    {
        if (_animator!=null)
            _animator.SetAiming(value);
    }
}
