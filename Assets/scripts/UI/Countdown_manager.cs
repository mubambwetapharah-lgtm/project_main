using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;

    void Start()
    {
        Time.timeScale = 0f; // останавливаем время
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        countdownText.text = "START!";
        yield return new WaitForSecondsRealtime(0.5f);

        // Плавное исчезновение
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.unscaledDeltaTime * 2f;
            countdownText.alpha = alpha;
            yield return null;
        }

        countdownText.alpha = 1f;
        gameObject.SetActive(false);
        Time.timeScale = 1f; // запускаем время
    }
}