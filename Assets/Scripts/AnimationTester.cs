using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationTester : MonoBehaviour
{
    [SerializeField] private float _interval = 5f;
    [SerializeField] private string _triggerName = "";

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(PlayAnimationLoop());
    }

    private IEnumerator PlayAnimationLoop()
    {
        _animator.SetTrigger(_triggerName);
        yield return new WaitForSeconds(_interval);
        yield return PlayAnimationLoop();
    }
}
