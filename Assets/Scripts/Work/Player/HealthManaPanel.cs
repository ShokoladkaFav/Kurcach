using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManaPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public Image healthBar;
    public Image manaBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Оновлення смужок
        healthBar.fillAmount = playerStats.currentHealth / playerStats.maxHealth;
        manaBar.fillAmount = playerStats.currentMana / playerStats.maxMana;

        // Оновлення тексту
        healthText.text = $"{Mathf.Floor(playerStats.currentHealth)} / {Mathf.Floor(playerStats.maxHealth)}";
        manaText.text = $"{Mathf.Floor(playerStats.currentMana)} / {Mathf.Floor(playerStats.maxMana)}";
    }
}
