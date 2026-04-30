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
    public float colliderSize = 0.15f;
    
    [Header("효과 (선택사항)")]
    public GameObject hitEffectPrefab;
    
    [Header("발사자정보")]
    public GameObject owner;

    private Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0f;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // AdjustColliderSize(); // 주석 처리 유지
    }

    void Start()
    {
        rb2D.linearVelocity = direction.normalized * speed;
        
        // ⭐ 방향 반전할 때 원래 크기 유지
        if (direction.x < 0)
        {
            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }

        // ⭐ 발사자와 충돌 무시
        if (owner != null)
        {
            Collider2D ownerCollider = owner.GetComponent<Collider2D>();
            Collider2D bulletCollider = GetComponent<Collider2D>();

            if (ownerCollider != null && bulletCollider != null)
            {
                Physics2D.IgnoreCollision(ownerCollider, bulletCollider, true);
                Debug.Log($"✅ 총알이 {owner.name}와(과) 충돌 무시 설정됨");
            }
        }
        
        // ⭐ 생명주기 동안만 유지 (한 번만 호출)
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ⭐ 발사자와 충돌 시 무시
        if (owner != null && collision.gameObject == owner)
        {
            Debug.Log($"🛡️ 발사자 {owner.name}와 충돌 - 무시됨");
            return;
        }

        // ⭐ 다른 플레이어와 충돌
        if (collision.gameObject.CompareTag("Player") && collision.gameObject != owner)
        {
            Debug.Log($"🎯 {collision.gameObject.name} 명중!");
            HandleCollision(collision.gameObject);
            return;
        }

        // 기타 오브젝트와 충돌
        ContactPoint2D contact = collision.GetContact(0);
        Debug.Log($"💥 정확한 충돌 지점: {contact.point}");
        HandleCollision(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ⭐ 발사자 무시
        if (owner != null && other.gameObject == owner)
            return;
            
        // ⭐ 다른 플레이어와 충돌
        if (other.CompareTag("Player") && other.gameObject != owner)
        {
            Debug.Log($"🎯 {other.gameObject.name} 명중! (트리거)");
            HandleCollision(other.gameObject);
            return;
        }
        
        if (!other.CompareTag("Player"))
        {
            HandleCollision(other.gameObject);
        }
    }

    void HandleCollision(GameObject hitObject)
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        Debug.Log($"총알이 {hitObject.name}와(과) 충돌하여 제거됩니다.");
        Destroy(gameObject);
    }

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