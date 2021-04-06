using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Bar
{
    [SerializeField] private Player _player;

    void Start()
    {
        _maxValue = _player.MaxHP;
        OnValueChanged(_maxValue);
    }

    private void OnEnable()
    {
        _player.HealthChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnValueChanged;
    }
}
