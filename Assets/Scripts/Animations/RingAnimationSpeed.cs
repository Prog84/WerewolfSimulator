using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class RingAnimationSpeed : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Start()
    {
        GetComponent<Animator>().speed = _speed;
    }
}
