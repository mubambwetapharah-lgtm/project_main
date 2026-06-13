using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("총알 속성")]
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 1;
    public Vector2 direction = Vector2.right;
    public GameObject owner;

    [Header("충돌 정밀도 설정")]
    public float colliderSize = 0.15f;

    [Header("효과 (선택사항)")]
    public GameObject hitEffectPrefab;

    private Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0f;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        AdjustColliderSize();
    }

    void Start()
    {
        rb2D.linearVelocity = direction.normalized * speed;

        // ✅ исправление напарника — сохраняем оригинальный масштаб
        if (direction.x < 0)
        {
            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }

        // ✅ игнорируем коллайдер владельца сразу при старте
        if (owner != null)
        {
            Collider2D ownerCollider = owner.GetComponent<Collider2D>();
            Collider2D bulletCollider = GetComponent<Collider2D>();
            if (ownerCollider != null && bulletCollider != null)
                Physics2D.IgnoreCollision(ownerCollider, bulletCollider, true);
        }

        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == owner) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        ContactPoint2D contact = collision.GetContact(0);
        Debug.Log($"💥 충돌 지점: {contact.point}");
        HandleCollision(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner) return;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        HandleCollision(other.gameObject);
    }

    void AdjustColliderSize()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col is CircleCollider2D circle)
            circle.radius = colliderSize;
        else if (col is BoxCollider2D box)
            box.size = new Vector2(colliderSize, colliderSize);
    }

    void HandleCollision(GameObject hitObject)
    {
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetOwner(GameObject ownerObject)
    {
        owner = ownerObject;
        Collider2D ownerCollider = ownerObject.GetComponent<Collider2D>();
        Collider2D bulletCollider = GetComponent<Collider2D>();
        if (ownerCollider != null && bulletCollider != null)
            Physics2D.IgnoreCollision(bulletCollider, ownerCollider);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col is CircleCollider2D circle)
            Gizmos.DrawWireSphere(transform.position, circle.radius);
    }
}