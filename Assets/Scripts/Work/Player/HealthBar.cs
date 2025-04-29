using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar UI")]
    public Image healthFill;
    public Vector3 offset = new Vector3(0, 2, 0);

    private Transform character;
    private PlayerStats playerStats;

    void Start()
    {
        character = transform.parent; // Canvas ������� �� ���������
        playerStats = character.GetComponent<PlayerStats>();
    }

    void Update()
    {
        // ��������� ������� Canvas
        transform.position = character.position + offset;

        // ��������� ������ ������'�
        float healthPercent = playerStats.currentHealth / playerStats.maxHealth;
        healthFill.fillAmount = healthPercent;
    }
}
