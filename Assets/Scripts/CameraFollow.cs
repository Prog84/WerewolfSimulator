using UnityEngine;
using System.Collections;
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Transform _player;

    void Update()
    {
        transform.position = _player.position + _offset;
    }
}