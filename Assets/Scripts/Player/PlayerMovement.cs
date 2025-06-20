using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] float movementSpeed = 100f;
    [SerializeField] float maxMovementSpeed = 5f;

    [SerializeField] Camera cam;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovementCommand = Input.GetAxis("Horizontal");
        float verticalMovementCommand = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2(horizontalMovementCommand * movementSpeed, verticalMovementCommand * movementSpeed);
        if (rb.linearVelocity.magnitude > maxMovementSpeed)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxMovementSpeed);
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + CursorRotation());
    }

    float CursorRotation()
    {
        Vector3 mousePosition = cam.ScreenPointToRay(Input.mousePosition).origin;
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0) - transform.position;

        return Vector3.SignedAngle(transform.right, mousePosition, Vector3.forward);
    }
}