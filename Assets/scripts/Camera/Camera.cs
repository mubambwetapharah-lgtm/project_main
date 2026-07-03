using UnityEngine;

// Повесить на Main Camera.
// НИЧЕГО не меняет в мире: ни фон, ни ящики, ни игроков.
// Вместо этого сама камера подстраивается под фон:
// 1) Orthographic Size выставляется так, чтобы высота фона идеально влезала.
// 2) Если аспект экрана не совпадает с аспектом фона — вместо
//    растяжения/обрезки добавляются пустые узкие полосы (letterbox/pillarbox)
//    по бокам или сверху/снизу через Camera.rect, а сам фон остаётся
//    видимым целиком, без искажений.
[RequireComponent(typeof(Camera))]
public class CameraFitBackground : MonoBehaviour
{
    [Tooltip("SpriteRenderer фона (объект Back). Его размер в мире — эталон, под который подстраивается камера.")]
    public SpriteRenderer background;

    [Tooltip("Цвет полос letterbox/pillarbox")]
    public Color letterboxColor = Color.black;

    private Camera cam;
    private Camera clearCam;
    private int lastScreenWidth;
    private int lastScreenHeight;

    void Start()
    {
        cam = GetComponent<Camera>();
        CreateClearCamera();
        Fit();
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }

    void CreateClearCamera()
    {
        // Служебная камера: рендерится ДО основной (меньший Depth),
        // ничего не рисует (Culling Mask = Nothing), но каждый кадр
        // заливает ВЕСЬ экран сплошным цветом — это гарантированно
        // убирает "застрявшие" пиксели предыдущего кадра/сцены в
        // зонах letterbox/pillarbox, которые Camera.rect сам не чистит.
        GameObject clearCamObj = new GameObject("LetterboxClearCamera");
        clearCamObj.transform.SetParent(transform);
        clearCam = clearCamObj.AddComponent<Camera>();
        clearCam.clearFlags = CameraClearFlags.SolidColor;
        clearCam.backgroundColor = letterboxColor;
        clearCam.cullingMask = 0; // ничего не рендерит
        clearCam.orthographic = true;
        clearCam.rect = new Rect(0f, 0f, 1f, 1f);
        clearCam.depth = cam.depth - 1f; // рисуется раньше основной камеры
        clearCam.allowHDR = false;
        clearCam.allowMSAA = false;
    }

    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            Fit();
        }
    }

    void Fit()
    {
        if (background == null || cam == null) return;

        // Реальный размер фона В МИРЕ (уже учитывает scale объекта и всех родителей)
        Bounds b = background.bounds;
        float bgWidth = b.size.x;
        float bgHeight = b.size.y;
        if (bgWidth <= 0f || bgHeight <= 0f) return;

        float targetAspect = bgWidth / bgHeight;

        // Высота камеры = ровно высота фона (в мировых юнитах)
        cam.orthographicSize = bgHeight / 2f;

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Rect rect = cam.rect;

        if (scaleHeight < 1f)
        {
            // Экран площе фона -> полосы сверху/снизу (letterbox)
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            // Экран шире фона -> полосы по бокам (pillarbox)
            float scaleWidth = 1f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;
        }

        cam.rect = rect;
    }
}