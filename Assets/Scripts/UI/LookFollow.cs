using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _levelDirection;
    [SerializeField] private float _distanse;
    [SerializeField] private PlayerInput _input;

    void Update()
    {
        transform.position = Vector3.Project(_player.position + (_input.Direction + _player.transform.forward).normalized * _distanse, _levelDirection + _player.position);
    }
}
