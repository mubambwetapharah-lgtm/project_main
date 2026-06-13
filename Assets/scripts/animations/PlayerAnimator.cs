using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = rb.linearVelocity.x;

        // Переключаем анимацию
        if (Mathf.Abs(horizontal) > 0.1f)
            anim.SetBool("IsRunning", true);
        else
            anim.SetBool("IsRunning", false);

        // Переворачиваем спрайт
        if (horizontal > 0.1f)
            sr.flipX = false; // вправо
        else if (horizontal < -0.1f)
            sr.flipX = true;  // влево
    }
}