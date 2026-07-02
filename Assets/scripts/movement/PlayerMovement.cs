using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private float horizontal;
    private InputAction moveAction;
    private Animator animator;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // страховка: физические удары не должны вращать/разворачивать игрока
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        moveAction = new InputAction(binding: "<Keyboard>/a");
        moveAction.AddBinding("<Keyboard>/d");
        moveAction.Enable();
    }

    void Update()
    {
        horizontal = 0f;
        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed) horizontal = 1f;

        animator.SetBool("IsRunning", horizontal != 0f);

        if (horizontal > 0f) sr.flipX = false;
        else if (horizontal < 0f) sr.flipX = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    void OnDestroy()
    {
        moveAction.Disable();
    }
}