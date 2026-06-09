using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GAME_SCENE"); // замени на имя своей игровой сцены
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}