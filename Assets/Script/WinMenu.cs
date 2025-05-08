using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public string loadGame;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level 2 City");
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(loadGame);
    }
    public void MainMenu()
    {
        //SceneManager.LoadScene("MenuMain");
        SceneManager.LoadScene("MenuMain");
    }
}
