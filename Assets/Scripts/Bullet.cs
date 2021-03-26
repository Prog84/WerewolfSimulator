using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;

    private float lifetime = 10;

    private void Update()
    {
        transform.Translate(transform.forward.normalized * _speed * Time.deltaTime, Space.World);
        lifetime -= Time.deltaTime;
        if (lifetime<0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
