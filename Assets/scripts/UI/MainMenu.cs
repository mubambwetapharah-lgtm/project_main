using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Клик мыши зафиксирован");

            // проверяем что под курсором
            var pointerData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            pointerData.position = Mouse.current.position.ReadValue();
            var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointerData, results);

            foreach (var r in results)
                Debug.Log($"Попал в: {r.gameObject.name}");
        }
    }
    public void StartGame()
    {
        Debug.Log("StartGame вызван!");
        SceneManager.LoadScene("GAME_SCENE"); // замени на имя своей игровой сцены
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame вызван!");
#if     UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}