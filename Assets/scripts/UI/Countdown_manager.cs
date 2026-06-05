using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public static bool isPaused = false;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);

        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            float timer = 1f;
            while (timer > 0f)
            {
                if (!isPaused) timer -= Time.unscaledDeltaTime;
                yield return null;
            }
        }

        countdownText.text = "START!";
        float startTimer = 0.5f;
        while (startTimer > 0f)
        {
            if (!isPaused) startTimer -= Time.unscaledDeltaTime;
            yield return null;
        }

        float alpha = 1f;
        while (alpha > 0f)
        {
            if (!isPaused) alpha -= Time.unscaledDeltaTime * 2f;
            countdownText.alpha = alpha;
            yield return null;
        }

        countdownText.alpha = 1f;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}