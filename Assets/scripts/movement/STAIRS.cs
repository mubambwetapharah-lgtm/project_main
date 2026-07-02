using UnityEngine;
using UnityEngine.InputSystem;

public class STAIRS : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool onStairs = false;
    private InputAction jumpAction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpAction = new InputAction(binding: "<Keyboard>/w");
        jumpAction.Enable();
    }

    void FixedUpdate()
    {
        if (!onStairs) return;

        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = jumpAction.IsPressed();

        if (!isMoving && !isJumping)
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
            onStairs = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
        {
            onStairs = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void OnDestroy()
    {
        jumpAction.Disable();
    }
}