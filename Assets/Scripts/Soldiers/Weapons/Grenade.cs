using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float _flyTime;
    [SerializeField] private float _height;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _damage;

    private Vector3 _target;
    private Vector3 _startPoint;
    private float _timer;

    public void InitTravel(Vector3 target)
    {
        _target = target;
        _target.y = 0;
        _startPoint = transform.position;
        _timer = 0;
    }

    void Update()
    {
        if (_timer > _flyTime)
            Explode();
        var target = Vector3.Lerp(_startPoint, _target, _timer / _flyTime);
        target.y += (1 - Mathf.Pow(2 * _timer / _flyTime - 1f, 2)) * _height;
        if (target != transform.position)
            transform.rotation = Quaternion.LookRotation(target - transform.position);
        transform.position = target;
        _timer += Time.deltaTime;
    }

    private void Explode()
    {
        Instantiate(_explosion, transform.position, transform.rotation);
        Destroy(gameObject);
        Collider[] targets = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Player player))
            {
                player.TakeDamage(_damage);
            }
        }
    }
}
