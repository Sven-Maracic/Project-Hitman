using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private float bulletSpeed;
    private PlayerStateController controller;

    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (controller = collision.GetComponent<PlayerStateController>())
        {
            controller.TakeDamage();
        }
        Destroy(this.gameObject);
    }

    public void GetValues(float bS)
    {
        bulletSpeed = bS;
    }
}
