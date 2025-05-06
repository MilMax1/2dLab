using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void PlayLevelOne()
    {
        SceneManager.LoadScene(5);
    }

    public void PlayLevelTwo()
    {
        SceneManager.LoadScene(1);
    }



    public void BackToMainMenu()
    {
        SceneManager.LoadScene(4);
    }
}
