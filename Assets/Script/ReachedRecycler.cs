using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject onCollectEffect;
    public float animationDuration = 1f; // Duration to wait before scene change

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure your player GameObject has the "Player" tag
        {
            StartCoroutine(ChangeSceneWithAnimation());
        }
    }

    private IEnumerator ChangeSceneWithAnimation()
    {
        if (onCollectEffect != null)
        {
            GameObject effect = Instantiate(onCollectEffect, transform.position, transform.rotation);
            Debug.Log("Effect spawned.");
            
            // Wait for the animation to play
            yield return new WaitForSeconds(animationDuration);
        }
        else
        {
            Debug.LogWarning("onCollectEffect is null.");
        }
        
        // Load the new scene
        SceneManager.LoadScene(sceneToLoad);
    }
}