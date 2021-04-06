using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBar : Bar
{
    [SerializeField] private Player _player;

    void Start()
    {
        _maxValue = _player.MaxRage;
        OnValueChanged(_maxValue);
    }

    private void OnEnable()
    {
        _player.RageChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _player.RageChanged -= OnValueChanged;
    }
}
