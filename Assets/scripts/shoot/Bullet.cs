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

    [Header("충돌 정밀도 설정")]
    public float colliderSize = 0.15f;  // ⭐ Collider 크기 조절
    
    [Header("효과 (선택사항)")]
    public GameObject hitEffectPrefab;
    
    private Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0f;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;  // ⭐ 정밀 충돌

        // ⭐ Collider 크기 자동 조정
        AdjustColliderSize();
    }

    void Start()
    {
        rb2D.linearVelocity = direction.normalized * speed;
        
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        // ⭐ 이 부분은 유지 (생성된 총알만 파괴됨)
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

         // ⭐ 충돌 지점 표시 (디버그)
        ContactPoint2D contact = collision.GetContact(0);
        Debug.Log($"💥 정확한 충돌 지점: {contact.point}");

        HandleCollision(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;
        HandleCollision(other.gameObject);
    }

    void AdjustColliderSize()
    {
        Collider2D col = GetComponent<Collider2D>();
        
        if (col is CircleCollider2D circle)
        {
            circle.radius = colliderSize;
        }
        else if (col is BoxCollider2D box)
        {
            box.size = new Vector2(colliderSize, colliderSize);
        }
        
        Debug.Log($"Collider 크기 조정됨: {colliderSize}");
    }

    void HandleCollision(GameObject hitObject)
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        Debug.Log($"총알이 {hitObject.name}와(과) 충돌하여 제거됩니다.");
        
        // ⭐ 중요: this.gameObject만 파괴 (프리팹 아님)
        Destroy(gameObject);
    }

    // ⭐ Scene View에서 Collider 크기 시각화
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col is CircleCollider2D circle)
        {
            Gizmos.DrawWireSphere(transform.position, circle.radius);
        }
    }
}