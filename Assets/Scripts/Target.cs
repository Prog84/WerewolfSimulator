using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Targeter _targeter;

    private MeshRenderer[] _meshes;
    private Transform _currentTarget;

    private bool _isON => _currentTarget != null;

    private void Awake()
    {
        _meshes = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnEnable()
    {
        _targeter.TargetChanged += OnTargetChanged;
    }

    private void OnDisable()
    {
        _targeter.TargetChanged -= OnTargetChanged;
    }

    private void HideMesh()
    {
        foreach (var mesh in _meshes)
        {
            mesh.enabled = false;
        }
    }

    private void ShowMesh()
    {
        foreach (var mesh in _meshes)
        {
            mesh.enabled = true;
        }
    }

    private void OnTargetChanged(Transform transformation)
    {
        _currentTarget = transformation;
        if (transformation == null)
        {
            HideMesh();
        }
        else
        {
            ShowMesh();
        }
    }

    private void Update()
    {
        if (_isON)
            transform.position = _currentTarget.position;
    }
}
