using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapon
{
    public override void Shoot(Transform shootPoint, Vector3 targetPosition)
    {
        var grenade = Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(Vector3.up));
        grenade.GetComponent<Grenade>().InitTravel(targetPosition);
        Instantiate(_muzzleFlash, shootPoint.position, shootPoint.rotation);
    }
}
