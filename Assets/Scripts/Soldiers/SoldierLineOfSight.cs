using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SoldierLineOfSight : MonoBehaviour
{
    [SerializeField] private float _viewRadius;
    [Range(0, 360)] [SerializeField] private float _viewAngle;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _meshResolution;
    [SerializeField] private Material[] _materials;

    public List<Transform> VisibleTargets = new List<Transform>();

    private Mesh _viewMesh;
    private MeshRenderer _meshRenderer;

    void Awake()
    {
        _viewMesh = new Mesh { name = "View Mesh" };
        GetComponent<MeshFilter>().mesh = _viewMesh;
        _meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(FindTargetsWithDelay(1f));
    }

    private void OnDisable()
    {
        if (TryGetComponent(out MeshRenderer mesh))
            mesh.enabled = false;
    }

    private void OnEnable()
    {
        if (TryGetComponent(out MeshRenderer mesh))
            mesh.enabled = true;
    }

    public void SetMaterial(int index)
    {
        if( _meshRenderer != null)
            _meshRenderer.material = _materials[index];
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void LateUpdate()
    {
        //DrawFieldOfView();
    }

    void FindVisibleTargets()
    {
        VisibleTargets.Clear();
            Collider[] targetsInViewRadius = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(transform.position, _viewRadius, targetsInViewRadius, _targetMask);
            for (int i = 0; i < size; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                    {
                        VisibleTargets.Add(target);
                    }
                }
            }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(_viewAngle * _meshResolution);
        float stepAngleSize = _viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        float maxDist = -1;
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = -_viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            if (newViewCast.dst > maxDist)
            {
                maxDist = newViewCast.dst;
            }

            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle);
        dir = transform.rotation * dir;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, _obstacleMask))
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, transform.position + dir * _viewRadius, _viewRadius, globalAngle);
            }
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
}