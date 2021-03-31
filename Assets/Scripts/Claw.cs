using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Claw : MonoBehaviour
{
    [SerializeField] private ParticleSystem _blood;

    private Transform _player;
    private SphereCollider _collider;
    private ParticleSystem.EmissionModule _particle;

    public Transform Player => _player;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _particle = GetComponent<ParticleSystem>().emission;
        if (transform.root.TryGetComponent(out Player player))
            _player = player.transform;
    }

    private void OnEnable()
    {
        _collider.enabled = true;
        _particle.enabled = true;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
        _particle.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Soldier soldier))
        {
            Instantiate(_blood, transform.position, transform.rotation);
        }
    }
}
