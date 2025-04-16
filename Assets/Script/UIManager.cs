using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private int canCount = 0;
    public TextMeshProUGUI cansCollectedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddCan()
    {
        canCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        cansCollectedText.text = "" + canCount;
    }
}
