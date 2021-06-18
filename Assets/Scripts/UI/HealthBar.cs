using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Bar
{
    //[SerializeField] private Player _player;
    [SerializeField] private Enemies _enemies;

    void Start()
    {
        _maxValue = _enemies._enemies.Count;//_player.MaxHP;
        OnValueChanged(0);
    }

    private void OnEnable()
    {
        _enemies.Died += OnValueChanged;
        //_player.HealthChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _enemies.Died += OnValueChanged;
        //_player.HealthChanged -= OnValueChanged;
    }
}
