using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemies : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PopupsSpawner _effectSpawner;
    [SerializeField] private Popup _killEffect;
    [SerializeField] private GameObject _boss;
    [SerializeField] private Wolf _wolf;
    [SerializeField] private ParticleSystem _ps;
    [SerializeField] private ParticleSystem _pLeftRainbow;
    [SerializeField] private ParticleSystem _pRightRainbow;

    public List<Soldier> _enemies;
    private List<Wolf> _wolfies;
    private float _enemiesDied;
    private bool _isFinish = false;

    public event UnityAction<float> Died;

    private void Awake()
    {
        _enemies = new List<Soldier>();
        _wolfies = new List<Wolf>();
        foreach (var enemy in GetComponentsInChildren<Soldier>())
        {
            _enemies.Add(enemy);
            enemy.Died += OnEnemyKilled;
        }
    }

    private void Update()
    {
        if (_enemies.Count == 0 && _isFinish == false)
        {
            Died?.Invoke(_enemiesDied);
            _isFinish = true;
        }
    }

    private void OnEnemyKilled(Soldier enemy)
    {
        enemy.Died -= OnEnemyKilled;
        var wolf = Instantiate(_wolf, enemy.transform.position, Quaternion.identity);
        _wolfies.Add(wolf);
        _enemies.Remove(enemy);
        _enemiesDied++;
        Died?.Invoke(_enemiesDied);
        _player.RiseRage(1);
        _effectSpawner.SpawnPopup(_killEffect, enemy.transform.position);
        if (_enemies.Count == 4)
        {
            for (int i = 0; i < _wolfies.Count; i++)
            {
                if (i % 2 == 0 )
                {
                    _wolfies[i].Die();
                }
            }

        }
    }
}
