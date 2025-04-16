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

        // Check if the other object has a PlayerController2D component
        if (other.GetComponent<PlayerController>() != null)
        {
            // Destroy the collectible
            Destroy(gameObject);

            // Instantiate the particle effect
            Instantiate(onCollectEffect, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(onCollectSound, Camera.main.transform.position, soundVolume);
            UIManager.Instance.AddCan();
        }
    }
}


