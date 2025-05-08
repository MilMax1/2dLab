using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;        // Assign in Inspector
    [TextArea] public string fullText;     // Your message
    public float baseDelay = 0.05f;        // Delay per character
    public float punctuationPause = 0.3f;  // Extra pause on . and !
    public float spacePause = 0.1f;        // Optional small pause on space

    private void Start()
    {
        textMeshPro.text = "";
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        foreach (char c in fullText)
        {
            if (c == '\t')
                continue; // Skip tabs

            textMeshPro.text += c;

            // Determine delay
            float delay = baseDelay;

            if (c == '.' || c == '!' || c == '?')
                delay += punctuationPause;
            else if (c == ' ')
                delay = spacePause;

            yield return new WaitForSeconds(delay);
        }
    }
}
