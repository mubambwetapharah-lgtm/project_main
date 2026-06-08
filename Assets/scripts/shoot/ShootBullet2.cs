using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShootBullet2 : MonoBehaviour
{
    [Header("총알 설정")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    [Header("발사 위치 자동 설정")]
    public Vector2 firePointOffset = new Vector2(0.5f, 0f);
    public bool showFirePointGizmo = true;

    [Header("발사 설정")]
    public float fireRate = 0.5f;
    public enum FireButton { MouseLeft, MouseRight, Space, J, K, Z, X }
    public FireButton fireButton = FireButton.MouseLeft;

    private float nextFireTime = -1f;
    private Transform firePoint;
    private GameObject cachedBulletPrefab;
    private InputAction fireAction;
    private SpriteRenderer spriteRenderer;  // ✅ 여기로 이동 (클래스 레벨)

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        CreateFirePoint();
        cachedBulletPrefab = bulletPrefab;
        StartCoroutine(WakeUpPrefab());

        fireAction = CreateFireAction(fireButton);
        fireAction.Enable();

        if (cachedBulletPrefab == null)
            Debug.LogError("Bullet Prefab이 Inspector에 설정되지 않았습니다!");
        
        if (spriteRenderer == null)
            Debug.LogWarning("SpriteRenderer를 찾을 수 없습니다! Flip 검사를 하려면 SpriteRenderer가 필요합니다.");
    }

    InputAction CreateFireAction(FireButton button)
    {
        string binding = button switch
        {
            FireButton.MouseLeft => "<Mouse>/leftButton",
            FireButton.MouseRight => "<Mouse>/rightButton",
            FireButton.Space => "<Keyboard>/space",
            FireButton.J => "<Keyboard>/j",
            FireButton.K => "<Keyboard>/k",
            FireButton.Z => "<Keyboard>/z",
            FireButton.X => "<Keyboard>/x",
            _ => "<Mouse>/leftButton"
        };
        return new InputAction(binding: binding);
    }

    void Update()
    {
        UpdateFirePointDirection();

        if (fireAction.IsPressed() && Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }
    }

    void OnDestroy()
    {
        fireAction.Disable();
    }

    IEnumerator WakeUpPrefab()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject temp = Instantiate(bulletPrefab, new Vector3(-9999, -9999, 0), Quaternion.identity);
        Destroy(temp);
    }

    void CreateFirePoint()
    {
        GameObject firePointObj = new GameObject("FirePoint_Auto");
        firePointObj.transform.SetParent(transform);
        firePointObj.transform.localPosition = firePointOffset;
        firePoint = firePointObj.transform;
    }

    void UpdateFirePointDirection()
    {
        if (firePoint == null) return;
        
        // ✅ 중복 선언 제거
        bool isFacingLeft = false;
        
        if (spriteRenderer != null)
            isFacingLeft = spriteRenderer.flipX;
        else
            isFacingLeft = transform.localScale.x < 0;
        
        Vector2 offset = firePointOffset;
        if (isFacingLeft)
            offset.x = -Mathf.Abs(firePointOffset.x);
        else
            offset.x = Mathf.Abs(firePointOffset.x);
        
        firePoint.localPosition = offset;
    }

    void FireBullet()
    {
        GameObject prefabToUse = bulletPrefab != null ? bulletPrefab : cachedBulletPrefab;
        if (prefabToUse == null)
        {
            Debug.LogError("Bullet Prefab이 null입니다! Inspector에서 다시 설정하십시오.");
            return;
        }
        if (firePoint == null)
        {
            Debug.LogError("Fire Point가 생성되지 않았습니다!");
            return;
        }
        
        // ✅ 방향 결정 (중복 제거)
        float direction = 1f;
        
        if (spriteRenderer != null)
        {
            if (spriteRenderer.flipX)
                direction = -1f;
            else
                direction = 1f;
        }
        else
        {
            direction = transform.localScale.x > 0 ? 1f : -1f;
        }
        
        // ✅ 총알 생성 (중복 제거)
        GameObject bullet = Instantiate(prefabToUse, firePoint.position, Quaternion.identity);
        
        // ✅ 크기 고정
        Vector3 fixedScale = prefabToUse.transform.localScale;
        fixedScale.x = Mathf.Abs(fixedScale.x);
        fixedScale.y = Mathf.Abs(fixedScale.y);
        bullet.transform.localScale = fixedScale;
        
        // ✅ Bullet 스크립트 설정
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = bulletSpeed;
            bulletScript.direction = new Vector2(direction, 0);
            bulletScript.owner = gameObject;
        }
        
        // ✅ 오디오 재생
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null) audioSource.Play();
    }

    void OnDrawGizmos()
    {
        if (!showFirePointGizmo) return;
        Gizmos.color = Color.red;
        Vector3 worldOffset = transform.TransformPoint(firePointOffset);
        Gizmos.DrawWireSphere(worldOffset, 0.01f);
        Gizmos.DrawLine(transform.position, worldOffset);
    }
}