using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public int totalCollectibles = 10;
    private int canCount = 0;
    public TextMeshProUGUI cansCollectedText;
    public TextMeshProUGUI healthText;

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
    public bool AllCollectibles()
    {
        return canCount >= totalCollectibles;
    }
    public void UpdateHealth(int currentHealth)
    {
        healthText.text = "" + currentHealth;
    }

    void UpdateUI()
    {
        cansCollectedText.text = "" + canCount+"/10";
    }
}
