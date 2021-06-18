using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthContainer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Arm arm))
        {
            //Vector3 direction = transform.position - collision.gameObject.transform.position;
            //direction.y = 0;
            //_rigidbody.AddForce(direction.normalized * 160, ForceMode.Impulse);
            _animator.SetTrigger("Die");
        }
    }
}
