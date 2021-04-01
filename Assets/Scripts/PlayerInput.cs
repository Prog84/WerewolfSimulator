using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    public bool IsON;

    private Player _player;

    public Vector3 Direction => new Vector3(Horizontal, 0, Vertical);

    private void Awake()
    {
        _player = GetComponent<Player>();
        IsON = true;
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
            if (IsON)
                return _joystick.Horizontal;
            else
                return 0;
        }
    }
    public float Vertical
    {
        get
        {
            if (IsON)
                return _joystick.Vertical;
            else
                return 0;
        }
    }

    public void TurnOff()
    {
        IsON = false;
    }

    public void TurnOn()
    {
        IsON = true;
    }
}
