using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Targeter _targeter;

    private Transform _currentTarget;
    private Transform _lastTarget;
    private Light _light;
    private Coroutine _changing;
    private Vector3 _baseScale;
    private float _baseRange;
    private float _currentLerp;

    private bool _isON => _currentTarget != null;

    private void Awake()
    {
        _light = GetComponentInChildren<Light>();
        if (_light == null)
            Debug.LogError("no light found");
        _baseScale = transform.localScale;
        _baseRange = _light.range;
    }

    private void OnEnable()
    {
        _targeter.TargetChanged += OnTargetChanged;
        _targeter.TurnedOFF += Hide;
    }

    private void OnDisable()
    {
        _targeter.TargetChanged -= OnTargetChanged;
        _targeter.TurnedOFF -= Hide;
    }

    private void Hide()
    {
        if (_changing != null)
            StopCoroutine(_changing);
        _changing = StartCoroutine(HideAll(0.7f));
    }

    private void Show()
    {
        if (_changing != null)
            StopCoroutine(_changing);
        _changing = StartCoroutine(ShowAll(0.3f));
    }

    private IEnumerator HideAll(float time)
    {
        float timer = time * (1f -_currentLerp);
        while (timer <= time)
        {
            timer += Time.deltaTime;
            SetScale((time - timer) / time);
            yield return null;
        }
    }
    private IEnumerator ShowAll(float time)
    {
        float timer = time * _currentLerp;
        while (timer <= time)
        {
            timer += Time.deltaTime;
            SetScale(timer / time);
            yield return null;
        }
    }

    private void SetScale (float lerp)
    {
        _currentLerp = lerp;
        transform.localScale = Vector3.Lerp(Vector3.zero, _baseScale, lerp);
        _light.range = Mathf.Lerp(0, _baseRange, lerp);
    }

    private void OnTargetChanged(Transform transformation)
    {
        if (_currentTarget != null)
            _lastTarget = _currentTarget;
        _currentTarget = transformation;
        if (transformation == null)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Update()
    {
        if (_isON)
            transform.position = _currentTarget.position;
        else if (_lastTarget != null)
            transform.position = _lastTarget.position;
    }
}
