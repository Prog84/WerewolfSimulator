using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack", order = 51)]
public class Attack : ScriptableObject
{
    [SerializeField] private PlayerAttacker.AttackSpeed _attackSpeed;
    [SerializeField] private bool _leftClaw;
    [SerializeField] private bool _rightClaw;
    [SerializeField] private float _pauseClawOn;
    [SerializeField] private float _pauseClawOff;
    [SerializeField] private float _pauseTargetHit;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _offset;

    public float Time =>  _pauseClawOff + PauseClawOn + _pauseTargetHit;
    public bool LeftClaw => _leftClaw;
    public bool RightClaw => _rightClaw;
    public float MinSpeed => _minSpeed;
    public float MaxSpeed => _maxSpeed;
    public float PauseClawOn => _pauseClawOn;
    public float PauseClawOff => _pauseClawOff;
    public float PauseTargetHit => _pauseTargetHit;
    public PlayerAttacker.AttackSpeed AttackSpeed => _attackSpeed;

    public float Offset => _offset;


}