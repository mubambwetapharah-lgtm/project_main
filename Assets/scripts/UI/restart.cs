using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class restart : MonoBehaviour
{
    [Header("버튼 설정")]
    public UnityEngine.UI.Button restartButton;

    void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(() => RestartGame());
        else
            Debug.LogWarning("Restart 버튼이 Inspector에 연결되지 않았습니다!");
    }

    void Update()
    {
        // R 키 또는 ESC 키로 재시작
        if (Keyboard.current != null)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Debug.Log("키 입력 감지됨 - 재시작 실행");
                RestartGame();
            }
        }
    }

    public void RestartGame()
    {
        Debug.Log("=== 게임 재시작 ===");
        
        // 시간 스케일 정상화
        Time.timeScale = 1f;
        
        // 방법 1: 씬 이름으로 로드 (가장 안정적)
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        
        // 방법 2: 인덱스로 로드 (빌드 설정 필요)
        // SceneManager.LoadScene(0);
    }
}