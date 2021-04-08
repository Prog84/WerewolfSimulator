using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float _range;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected ParticleSystem _muzzleFlash;
    [SerializeField] protected float _rateOfFire;

    public float Range => _range;
    public float RateofFire => _rateOfFire;

    public abstract void Shoot(Transform shootPoint, Vector3 target);

}
