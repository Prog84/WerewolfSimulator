using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField] private int _bulletsPerShot;
    [SerializeField] private float _delay;

    public override void Shoot(Transform shootPoint, Vector3 targetPosition)
    {
        var random = new Vector3(0.4f, 0.2f, 0.4f);
        Vector3 direction = (targetPosition - shootPoint.position).normalized;
        direction.x += Random.Range(-random.x, random.x);
        direction.y += Random.Range(-random.y, random.y);
        direction.z += Random.Range(-random.z, random.z);
        StartCoroutine(MakeShots(shootPoint, direction, _bulletsPerShot, _delay));
    }

    private IEnumerator MakeShots(Transform shootPoint, Vector3 direction, int amount, float pause)
    {
        for (var i=0; i<amount; i++)
        {
            SpawnBullet(shootPoint, direction);
            yield return new WaitForSeconds(pause);
        }
    }

    private void SpawnBullet(Transform shootPoint, Vector3 direction)
    {
        Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(direction));
        Instantiate(_muzzleFlash, shootPoint.position, shootPoint.rotation);
    }
}
