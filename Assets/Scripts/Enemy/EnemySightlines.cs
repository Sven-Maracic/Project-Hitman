using System.Linq;
using UnityEngine;

public class EnemySightlines : MonoBehaviour
{
    private Mesh mesh;
    private PolygonCollider2D polygonCollider;

    [SerializeField] Transform enemyTransform;
    [SerializeField] EnemyStateController stateController;
    [SerializeField] float FOV = 90f;
    [SerializeField] float viewDistance = 5f;
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] int rayCount = 50;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        polygonCollider = GetComponent<PolygonCollider2D>();
        GetComponent<MeshFilter>().mesh = mesh;
        transform.position = Vector3.zero;
    }
    private void Update()
    {
        if (stateController.currentState == EnemyStateController.AvailableStates.Dead)
        {
            Destroy(gameObject);
        }
        Vector3 origin = enemyTransform.position;
        Debug.DrawRay(origin, enemyTransform.right, Color.magenta);
        float angle = LookWhereEnemyLooking() - 90 + FOV/2;//- 45;
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
        polygonCollider.points = verticies.Select((v3) => (Vector2)v3).ToArray();

    }

    float LookWhereEnemyLooking()
    {
        return Vector3.SignedAngle(-transform.up, enemyTransform.right, Vector3.forward);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stateController.UpdateState(EnemyStateController.AvailableStates.Chasing);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stateController.UpdateState(EnemyStateController.AvailableStates.Lost);
        }
    }

}
