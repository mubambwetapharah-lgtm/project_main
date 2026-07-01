using UnityEngine;

public class STAIRS : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool onStairs = false;
    private Vector2 lastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (onStairs && Mathf.Abs(rb.linearVelocity.x) < 0.1f)
        {
            rb.MovePosition(lastPosition);
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            lastPosition = rb.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            onStairs = true;
            lastPosition = rb.position;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
            onStairs = false;
    }
}