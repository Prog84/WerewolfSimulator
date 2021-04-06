using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Bar : MonoBehaviour
{
    [SerializeField] private float _changeTime;

    protected float _maxValue;
    private Slider _bar;
    private Coroutine _changing;

    private void Awake()
    {
        _bar = GetComponent<Slider>();
    }
    protected void OnValueChanged(float value)
    {
        StartChangeValue(value / _maxValue, _changeTime);
    }

    private IEnumerator ChangeValue(float target, float time)
    {
        float timer = 0;
        while (timer <= time)
        {
            _bar.value = Mathf.Lerp(_bar.value, target, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void StartChangeValue(float target, float time)
    {
        if (_changing != null)
            StopCoroutine(_changing);
        _changing = StartCoroutine(ChangeValue(target, time));
    }

}
