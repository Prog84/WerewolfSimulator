using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private ParticleSystem _blood;

    private float lifetime = 5;

    private void Update()
    {
        transform.Translate(transform.forward.normalized * _speed * Time.deltaTime, Space.World);
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            player.TakeDamage(_damage);
            Instantiate(_blood, transform.position, transform.rotation, player.transform);
        }
        Destroy(gameObject);
    }
}
