using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 5f;         
    public float viewAngle = 90f;       
    public LayerMask obstacleMask;       

    private Mesh viewMesh;
    public int rayCount = 50;            
    public Material coneMaterial;  
    
    public LayerMask targetMask; 
    private List<GameObject> visibleTargets = new List<GameObject>();

    void Start()
    {
        viewMesh = new Mesh();
        GameObject cone = new GameObject("FieldOfViewCone");
        cone.transform.parent = transform;
        cone.AddComponent<MeshFilter>().mesh = viewMesh;
        cone.AddComponent<MeshRenderer>().material = coneMaterial;
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }
    
    void Update()
    {
        FindVisibleTargets();
    }
    
    public void SetConeParameters(float radius, float angle)
    {
        viewRadius = radius;
        viewAngle = angle;
    }

    public void ToggleCone(bool isActive)
    {
        GetComponent<MeshRenderer>().enabled = isActive;
    }
    

    void DrawFieldOfView()
    {
        float angleStep = viewAngle / rayCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = transform.eulerAngles.z - viewAngle / 2 + angleStep * i;
            viewPoints.Add(GetRaycastPoint(angle));
        }

        CreateMesh(viewPoints);
    }
    
    void FindVisibleTargets()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        foreach (var target in targets)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, viewRadius, obstacleMask);
                if (hit.collider == null || hit.collider.gameObject == target.gameObject)
                {
                    if (!visibleTargets.Contains(target.gameObject))
                    {
                        visibleTargets.Add(target.gameObject);
                        target.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
            else
            {
                if (visibleTargets.Contains(target.gameObject))
                {
                    visibleTargets.Remove(target.gameObject);
                    target.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }

    Vector3 GetRaycastPoint(float angle)
    {
        Vector3 dir = DirFromAngle(angle);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);

        if (hit.collider != null)
        {
            return hit.point;
        }
        else
        {
            return (Vector2)transform.position + (Vector2)dir * viewRadius;
        }
    }

    void CreateMesh(List<Vector3> viewPoints)
    {
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < viewPoints.Count; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < viewPoints.Count - 1)
            {
                int triangleIndex = i * 3;
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = i + 1;
                triangles[triangleIndex + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    Vector3 DirFromAngle(float angleInDegrees)
    {
        float rad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}
