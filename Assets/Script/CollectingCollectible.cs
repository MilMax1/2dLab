using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectCollectible : MonoBehaviour
{
    public GameObject onCollectEffect;
    public AudioClip onCollectSound;
    public float soundVolume = 1f;


    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Trigger entered by player!");

            if (onCollectEffect != null)
            {
                Instantiate(onCollectEffect, transform.position, transform.rotation);
                Debug.Log("Effect spawned.");
            }
            else
            {
                Debug.LogWarning("onCollectEffect is null.");
            }

            if (onCollectSound != null && Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(onCollectSound, Camera.main.transform.position, soundVolume);
                Debug.Log("Sound played.");
            }
            else
            {
                Debug.LogWarning("AudioClip or Camera.main is null.");
            }

            if (UIManager.Instance != null)
            {
                UIManager.Instance.AddCan();
                Debug.Log("Can added.");
            }
            else
            {
                Debug.LogWarning("UIManager.Instance is null.");
            }

            Destroy(gameObject);
        }
    }

}


