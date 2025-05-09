using UnityEngine;
using UnityEngine.SceneManagement;

public class Deatbox : MonoBehaviour
{
    public string loosemenu = "ParkLooseMenu";
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reload the current scene to "respawn"
            SceneManager.LoadScene(loosemenu);
        }
    }
}
