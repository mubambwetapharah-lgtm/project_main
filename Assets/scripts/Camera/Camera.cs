using UnityEngine;

// Повесить на фоновый объект (тот самый "Back" со SpriteRenderer).
// Растягивает спрайт так, чтобы он всегда полностью закрывал
// область видимости камеры, независимо от соотношения сторон экрана.
[RequireComponent(typeof(SpriteRenderer))]
public class FitBackgroundToCamera : MonoBehaviour
{
    [Tooltip("Если true — фон будет заполнять экран целиком (может немного обрезаться по краям). " +
             "Если false — фон впишется полностью, но по краям может остаться Background Color камеры.")]
    public bool cropToFill = true;

    void Start()
    {
        Fit();
    }

    void Fit()
    {
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (cam == null || sr == null || sr.sprite == null) return;

        // Реальный размер спрайта в локальных единицах при scale = 1
        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // Видимая область камеры в МИРОВЫХ единицах
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        // Масштаб родителя (если объект вложен, напр. в NEW MAP) —
        // без этого итоговый МИРОВОЙ размер будет неверным
        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;

        float worldScaleX = camWidth / spriteWidth;
        float worldScaleY = camHeight / spriteHeight;

        float finalWorldScale = cropToFill ? Mathf.Max(worldScaleX, worldScaleY) : Mathf.Min(worldScaleX, worldScaleY);

        // Переводим нужный МИРОВОЙ масштаб обратно в localScale с учётом родителя
        float localX = parentScale.x != 0 ? finalWorldScale / parentScale.x : finalWorldScale;
        float localY = parentScale.y != 0 ? finalWorldScale / parentScale.y : finalWorldScale;

        transform.localScale = new Vector3(localX, localY, 1f);
    }
}