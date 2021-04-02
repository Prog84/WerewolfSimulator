using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Claw : MonoBehaviour
{
    [SerializeField] private ParticleSystem _blood;

    private Transform _player;
    private SphereCollider _collider;
    private ParticleSystem _particle;

    public Transform Player => _player;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _particle = GetComponent<ParticleSystem>();
        if (transform.root.TryGetComponent(out Player player))
            _player = player.transform;
    }

    private void OnEnable()
    {
        _collider.enabled = true;
        var emission = _particle.emission;
        emission.enabled = true;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
        var emission = _particle.emission;
        emission.enabled = false;
        _particle.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Soldier soldier))
        {
            Instantiate(_blood, transform.position, transform.rotation);
        }
    }
}
