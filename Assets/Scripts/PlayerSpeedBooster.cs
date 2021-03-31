using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
public class PlayerSpeedBooster : MonoBehaviour
{
    [SerializeField] private float _speedPower;
    [SerializeField] private float _rotationPower;
    [SerializeField] private float _charging;
    [SerializeField] private float _decaying;
    [SerializeField] private float _maxAngle;

    private Quaternion _lastRotation;
    private PlayerInput _input;

    public float CurrentBoost => _currentBoost;
    public float CurrentSpeed => Mathf.Lerp(1, _speedPower, _currentBoost);
    public float CurrentRotation => 1f / Mathf.Lerp(1f, _rotationPower, _currentBoost);

    private float _currentMaxAngle => _maxAngle * CurrentRotation;

    public float _currentBoost;
    public float delta;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }
    private void FixedUpdate()
    {
        var angleChange = Quaternion.Angle(_lastRotation, transform.rotation);
        delta = _currentMaxAngle - angleChange;
        if (delta > 0 && _input.Direction.magnitude > 0.8f)
        {
            _currentBoost += Mathf.Lerp(0, _charging, delta / _currentMaxAngle * Time.fixedDeltaTime);
        }
        else
        {
            _currentBoost -= Mathf.Lerp(0, _decaying, Mathf.Abs(delta) / _currentMaxAngle * Time.fixedDeltaTime);
        }
        _currentBoost = Mathf.Clamp(_currentBoost, 0, 1);
        _lastRotation = transform.rotation;
    }


}
