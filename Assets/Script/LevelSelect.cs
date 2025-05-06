using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void PlayLevelOne()
    {
        SceneManager.LoadScene(3);
    }

    public void PlayLevelTwp()
    {
        SceneManager.LoadScene(3);
    }

    public void PlayLevelThree()
    {
        SceneManager.LoadScene(3);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
