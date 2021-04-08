using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _speed;

    private Transform _camera;
    private CanvasGroup _canvas;
    private float _activationTime;
    private float _timer;

    private void Awake()
    {
        enabled = false;
        _canvas = GetComponentInChildren<CanvasGroup>();
    }

    public void Init(Transform camera)
    {
        _timer = 0;
        _camera = camera;
        enabled = true;
    }

    private void Update()
    {
        if (_camera != null)
            Work();
    }

    private void Work()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);
        transform.rotation = _camera.rotation;
        _canvas.alpha = 1-(_timer/_lifeTime);
        _timer += Time.deltaTime;
        if (_timer > _lifeTime)
            Destroy(gameObject);
    }
}
