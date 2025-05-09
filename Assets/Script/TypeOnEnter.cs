using TMPro;
using UnityEngine;
using System.Collections;

public class TypeOnEnter : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;        // Assign in Inspector
    [TextArea] public string fullText;         // Your message
    public float baseDelay = 0.05f;            // Delay per character
    public float punctuationPause = 0.3f;      // Extra pause on . and !
    public float spacePause = 0.1f;            // Optional small pause on space

    private bool hasTriggered = false;         // Prevent retriggering

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            hasTriggered = true;
            textMeshPro.text = "";
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        foreach (char c in fullText)
        {
            if (c == '\t')
                continue; // Skip tabs

            textMeshPro.text += c;

            float delay = baseDelay;

            if (c == '.' || c == '!' || c == '?')
                delay += punctuationPause;
            else if (c == ' ')
                delay = spacePause;

            yield return new WaitForSeconds(delay);
        }
    }
}
