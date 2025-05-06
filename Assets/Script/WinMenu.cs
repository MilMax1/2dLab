using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public string loadGame;
    public string mainMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
public void PlayGame()
    {
        SceneManager.LoadScene(loadGame);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
