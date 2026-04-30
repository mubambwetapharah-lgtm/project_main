using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private float horizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input System의 Keyboard 클래스 직접 사용
        horizontal = 0f;
        
        if (Keyboard.current.leftArrowKey.isPressed)
            horizontal = -1f;
        if (Keyboard.current.rightArrowKey.isPressed)
            horizontal = 1f;
    }

    void FixedUpdate()
    {
        // 물리 기반 이동 처리
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
}