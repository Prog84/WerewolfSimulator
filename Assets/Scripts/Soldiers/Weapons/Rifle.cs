using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class projectile
{
    public string name;
    public Rigidbody bombPrefab;
    public GameObject muzzleflare;
    public float min, max;
    public bool rapidFire;
    public float rapidFireCooldown;

    public bool shotgunBehavior;
    public int shotgunPellets;
    public GameObject shellPrefab;
    public bool hasShells;
}
public class Rifle : Weapon
{
    [SerializeField] private int _bulletsPerShot;
    [SerializeField] private float _delay;
    public projectile bombList;

    public override void Shoot(Transform shootPoint, Vector3 targetPosition)
    {
        var random = new Vector3(0.1f, 0f, 0.1f);
        Vector3 direction = (targetPosition - shootPoint.position).normalized;
        /*direction.x += Random.Range(-random.x, random.x);*/
        direction.y += 0.2f/*Random.Range(-random.y, random.y)*/;
        /*direction.z += Random.Range(-random.z, random.z);*/
        StartCoroutine(MakeShots(shootPoint, direction, _bulletsPerShot, _delay));
    }

    private IEnumerator MakeShots(Transform shootPoint, Vector3 direction, int amount, float pause)
    {
        for (var i = 0; i < amount; i++)
        {
            SpawnBullet(shootPoint, direction);
            yield return new WaitForSeconds(pause);
        }
    }

    private void SpawnBullet(Transform shootPoint, Vector3 direction)
    {
        Rigidbody rocketInstance;
        rocketInstance = Instantiate(bombList.bombPrefab, shootPoint.position, Quaternion.LookRotation(direction)) as Rigidbody;

        rocketInstance.AddForce(direction * Random.Range(bombList.min, bombList.max));

        Instantiate(_muzzleFlash, shootPoint.position, Quaternion.LookRotation(direction)/*shootPoint.rotation*/);
        //Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(direction));
        //Instantiate(_muzzleFlash, shootPoint.position, Quaternion.LookRotation(direction)/*shootPoint.rotation*/);
    }
}
