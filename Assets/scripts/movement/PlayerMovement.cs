using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private InputAction moveAction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction = new InputAction(binding: "<Keyboard>/a");
        moveAction.AddBinding("<Keyboard>/d");
        moveAction.Enable();
    }

    void Update()
    {
        float horizontal = 0f;
        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed) horizontal = 1f;

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    void OnDestroy()
    {
        moveAction.Disable();
    }
}