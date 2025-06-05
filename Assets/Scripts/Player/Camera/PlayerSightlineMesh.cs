using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSightlineMesh : MonoBehaviour
{
    private Mesh mesh;
    private float startingAngle;
    Vector3 mousePosition;


    [SerializeField] Transform playerTransform;
    [SerializeField] Camera cam;
    [SerializeField] float FOV = 90f;
    [SerializeField] float viewDistance = 5f;
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] int rayCount = 50;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    private void Update()
    {
        Vector3 origin = playerTransform.position;
        float angle = CursorRotation()-45;
        float angleIncrease = FOV / rayCount;

        Vector3[] verticies = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[verticies.Length];
        int[] triangles = new int[rayCount * 3];

        verticies[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, viewDistance, ~ignoreLayers);

            if (hit.collider == null)
            {
                vertex = origin + direction * viewDistance;
            }
            else
            {
                vertex = hit.point;
            }

            verticies[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = verticies;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }
    float CursorRotation()
    {
        mousePosition = cam.ScreenPointToRay(Input.mousePosition).origin;
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0)  - playerTransform.position;

        return Vector3.SignedAngle(-transform.up, mousePosition, Vector3.forward);
    }
}
