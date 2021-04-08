using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemies : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PopupsSpawner _effectSpawner;
    [SerializeField] private Popup _killEffect;

    private List<Soldier> _enemies;

    public event UnityAction Died;

    private void Awake()
    {
        _enemies = new List<Soldier>();
        foreach (var enemy in GetComponentsInChildren<Soldier>())
        {
            _enemies.Add(enemy);
            enemy.Died += OnEnemyKilled;
        }
    }

    private void OnEnemyKilled(Soldier enemy)
    {
        enemy.Died -= OnEnemyKilled;
        _enemies.Remove(enemy);
        Died?.Invoke();
        _player.RiseRage(10);
        _effectSpawner.SpawnPopup(_killEffect, enemy.transform.position);
    }
}
