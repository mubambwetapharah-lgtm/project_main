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

        rb.freezeRotation = true; // страховка: физические удары не должны вращать/разворачивать игрока
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = true; // по умолчанию смотрит вправо

    }

    void Update()
    {

        // Input System의 Keyboard 클래스 직접 사용
        horizontal = 0f;
        
        if (Keyboard.current.leftArrowKey.isPressed)
            horizontal = -1f;
        if (Keyboard.current.rightArrowKey.isPressed)
            horizontal = 1f;

        horizontal = 0f;
        if (Keyboard.current.leftArrowKey.isPressed) horizontal = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) horizontal = 1f;

        animator.SetBool("IsRunning", horizontal != 0f);

        if (horizontal > 0f) sr.flipX = false;
        else if (horizontal < 0f) sr.flipX = true;

    }

    void FixedUpdate()
    {

        // 물리 기반 이동 처리

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
}