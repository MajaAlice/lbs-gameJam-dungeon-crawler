using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOptions()
    {
        SceneManager.LoadScene("Nah");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Demo_Level");
    }

    public void OptionsReturn()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
