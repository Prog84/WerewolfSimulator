using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4;
    [SerializeField] private float _rotateSpeed = 4;
    [SerializeField] private float _minSpeed = 0.2f;
    [SerializeField] private Joystick _joystick;

    private Rigidbody rb;

    public float MovingSpeed => new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).magnitude;
    public bool IsMoving => MovingSpeed > _minSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (IsMoving)
        {
            var direction = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
            MoveTo(direction);
            RotateTo(direction);
        }
    }

    void MoveTo(Vector3 pos)
    {
        rb.MovePosition(Vector3.Lerp(rb.position, rb.position + pos, Time.fixedDeltaTime * _moveSpeed));
    }

    private void RotateTo(Vector3 position)
    {
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(position), Time.fixedDeltaTime * _rotateSpeed));
    }
}
