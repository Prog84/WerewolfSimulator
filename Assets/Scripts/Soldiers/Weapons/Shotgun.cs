using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int _pellets;
    [SerializeField] private float _spread;
    
    public override void Shoot(Transform shootPoint, Vector3 targetPosition)
    {
        for (var i = 0; i < _pellets; i++)
        {
            Instantiate(bullet, shootPoint.position + Random.insideUnitSphere * _spread, Quaternion.LookRotation(targetPosition - shootPoint.position));
        }
        Instantiate(_muzzleFlash, shootPoint.position, shootPoint.rotation);
    }

    private void SpawnPellet(Transform shootPoint, Vector3 targetPosition)
    {
        Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(targetPosition - shootPoint.position));
    }
}
