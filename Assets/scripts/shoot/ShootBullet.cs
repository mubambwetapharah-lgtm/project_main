using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShootBullet : MonoBehaviour
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

    public System.Action OnFire;

    private float nextFireTime = -1f;
    private Transform firePoint;
    private GameObject cachedBulletPrefab;
    private InputAction fireAction;
    private SpriteRenderer spriteRenderer;

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

        // ✅ flipX от напарника, localScale как запасной вариант
        bool isFacingLeft = spriteRenderer != null ? spriteRenderer.flipX : transform.localScale.x < 0;

        Vector2 offset = firePointOffset;
        offset.x = isFacingLeft ? -Mathf.Abs(firePointOffset.x) : Mathf.Abs(firePointOffset.x);
        firePoint.localPosition = offset;
    }

    void FireBullet()
    {
        GameObject prefabToUse = bulletPrefab != null ? bulletPrefab : cachedBulletPrefab;
        if (prefabToUse == null)
        {
            Debug.LogError("Bullet Prefab이 null입니다!");
            return;
        }
        if (firePoint == null)
        {
            Debug.LogError("Fire Point가 생성되지 않았습니다!");
            return;
        }

        // ✅ flipX от напарника, localScale как запасной вариант
        float direction = spriteRenderer != null
            ? (spriteRenderer.flipX ? -1f : 1f)
            : (transform.localScale.x > 0 ? 1f : -1f);

        GameObject bullet = Instantiate(prefabToUse, firePoint.position, Quaternion.identity);

        // ✅ фиксация масштаба пули от напарника
        Vector3 fixedScale = prefabToUse.transform.localScale;
        fixedScale.x = Mathf.Abs(fixedScale.x);
        fixedScale.y = Mathf.Abs(fixedScale.y);
        bullet.transform.localScale = fixedScale;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = bulletSpeed;
            bulletScript.direction = new Vector2(direction, 0);
            bulletScript.SetOwner(this.gameObject);
        }


        OnFire?.Invoke();
        
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