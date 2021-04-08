using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BarbedWire : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private ParticleSystem _blood;
    [SerializeField] private int _bloodAmount;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            player.HitFront();
            for (var i=0; i<_bloodAmount; i++)
            {
                Instantiate(_blood, collider.bounds.center, Quaternion.LookRotation(Random.onUnitSphere), player.transform);
            }
            player.TakeDamage(_damage);
        }
    }
}
