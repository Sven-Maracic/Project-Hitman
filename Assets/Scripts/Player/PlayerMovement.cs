using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] float movementSpeed = 100f;
    [SerializeField] float maxMovementSpeed = 5f;

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
    }

}