using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class restart : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public Button continueButton;
    public Button pauseRestartButton;
    public Button pauseMainMenuButton;

    [Header("Death Menu")]
    public GameObject deathMenu;
    public Button deathRestartButton;
    public Button deathMainMenuButton;

    [Header("Countdown")]
    public GameObject countdownObject;
    public TextMeshProUGUI countdownText;

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        countdownObject.SetActive(false);

        continueButton.onClick.AddListener(ContinueGame);
        pauseRestartButton.onClick.AddListener(() => RestartGame());
        pauseMainMenuButton.onClick.AddListener(GoToMainMenu);

        deathRestartButton.onClick.AddListener(() => RestartGame());
        deathMainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ContinueGame();
            else PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void ContinueGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowDeathMenu()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // назови сцену главного меню
    }

    void RestartGame()
    {
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}