using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private float horizontal;
    private Animator animator;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = true; // по умолчанию смотрит вправо
    }

    void Update()
    {
        horizontal = 0f;
        if (Keyboard.current.leftArrowKey.isPressed) horizontal = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) horizontal = 1f;

        animator.SetBool("IsRunning", horizontal != 0f);

        if (horizontal > 0f) sr.flipX = false;
        else if (horizontal < 0f) sr.flipX = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
}