using UnityEngine;

public class PlaySoundOnXPass : MonoBehaviour
{
    public float targetX = 5.0f;               // X value to trigger the sound
    public AudioClip soundToPlay;              // Assign this in the Inspector

    private AudioSource audioSource;
    private float previousX;

    void Start()
    {
        previousX = transform.position.x;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        float currentX = transform.position.x;

        // Check if we crossed the target X from left to right
        if (targetX > previousX && targetX <= currentX)
        {
            audioSource.PlayOneShot(soundToPlay);
        }

        previousX = currentX;
    }
}
