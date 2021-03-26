using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 4;
    [SerializeField] private Joystick _joystick;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveTo(new Vector3(_joystick.Horizontal, 0, _joystick.Vertical));
    }

    void MoveTo(Vector3 pos)
    {
        rb.MovePosition(Vector3.Lerp(rb.position, rb.position + pos, Time.fixedDeltaTime * _movementSpeed));
    }
}
