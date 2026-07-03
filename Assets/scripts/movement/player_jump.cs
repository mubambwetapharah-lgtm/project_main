using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Управление")]
    [Tooltip("Путь к клавише в Input System, например \"<Keyboard>/w\" или \"<Keyboard>/upArrow\"")]
    [SerializeField] private string jumpKeyBinding = "<Keyboard>/w";

    [Header("Прыжок")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Проверка земли")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    // Подписывайтесь на это событие, чтобы реагировать на прыжок (например, звук)
    public event Action OnJump;

    private Rigidbody2D rb;
    private InputAction jumpAction;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpAction = new InputAction(binding: jumpKeyBinding);
        jumpAction.Enable();
    }

    void Update()
    {
        if (jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);

            OnJump?.Invoke(); // 🔔 сигнал для подписчиков (например, аудио)
        }

        if (jumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Обновляем VelocityY каждый кадр
        animator.SetFloat("VelocityY", rb.linearVelocity.y);

        // Приземлились — сбрасываем прыжок
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