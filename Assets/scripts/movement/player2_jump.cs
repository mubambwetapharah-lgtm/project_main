using UnityEngine;
using UnityEngine.InputSystem;

public class Player2_Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Проверка земли")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private InputAction jumpAction;
    private Animator animator;
    public System.Action OnJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpAction = new InputAction(binding: "<Keyboard>/upArrow");
        jumpAction.Enable();
    }

    void Update()
    {
        if (jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);

            OnJump?.Invoke();
        }

        if (jumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        animator.SetFloat("VelocityY", rb.linearVelocity.y);

        if (IsGrounded() && !jumpAction.WasPressedThisFrame())
        {
            animator.SetBool("IsJumping", false);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDestroy()
    {
        jumpAction.Disable();
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}