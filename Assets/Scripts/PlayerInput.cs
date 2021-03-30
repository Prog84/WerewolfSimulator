using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    private Player _player;
    private bool _isON;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _isON = true;
    }

    private void OnEnable()
    {
        _player.Died += TurnOff;
    }

    private void OnDisable()
    {
        _player.Died -= TurnOff;
    }

    public float Horizontal
    {
        get
        {
            if (_isON)
                return _joystick.Horizontal;
            else
                return 0;
        }
    }
    public float Vertical
    {
        get
        {
            if (_isON)
                return _joystick.Vertical;
            else
                return 0;
        }
    }

    public void TurnOff()
    {
        _isON = false;
    }

    public void TurnOn()
    {
        _isON = true;
    }
}
