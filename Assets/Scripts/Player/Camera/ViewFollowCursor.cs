using UnityEngine;

public class ViewFollowCursor : MonoBehaviour
{

    Mesh mesh;

    [SerializeField] float FOV = 90f;
    [SerializeField] int rayCount = 50;
    [SerializeField] float viewDistance = 5f;
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] Camera cam;

    Vector3 mousePosition;
    Vector3 direction;
    Vector3 origin = Vector3.zero;
    Vector3 vertex;
    float angle;
    float angleIncrease;
    RaycastHit2D hit;

    Vector3[] verticies;
    Vector2[] uv;
    int[] triangles;

    int vertexIndex = 1;
    int triangleIndex = 0;

    /*
    
    // Update is called once per frame
    void Update()
    {
        

    }*/

    private void Start()
    {
        angleIncrease = FOV / rayCount;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        verticies = new Vector3[rayCount + 2];
        uv = new Vector2[verticies.Length];
        triangles = new int[rayCount * 3];

        verticies[0] = origin;
        vertexIndex = 1;
        triangleIndex = 0;
        angle = -45 + CursorRotation();

        for (int i = 0; i <= rayCount; i++)
        {
            direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            hit = Physics2D.Raycast(transform.position, direction, viewDistance, ~ignoreLayers);
            if (hit.collider == null)
            {
                vertex = origin + direction * viewDistance;
            }
            else
            {
                Debug.DrawLine(transform.position, hit.point, Color.blue, 5f);
                vertex = hit.point;
            }
            verticies[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
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

    }

    float CursorRotation()
    {
        mousePosition = cam.ScreenPointToRay(Input.mousePosition).origin - transform.position;
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        return Vector3.SignedAngle(-transform.up, mousePosition, Vector3.forward);
    }
}