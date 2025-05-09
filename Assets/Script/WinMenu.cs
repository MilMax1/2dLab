using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public string loadGame;
<<<<<<< HEAD
=======
    public bool isPaused;
>>>>>>> 64b317b9d28a4dde21bdb47c076accefea7fc88f

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (UIManager.Instance.AllCollectibles())
            {
                Debug.Log("You win!");
                // Example: load next level or show win screen
                // SceneManager.LoadScene("NextLevel");
            }
        }
    }
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
        Time.timeScale = 1f;
        isPaused = false;
    }
}
