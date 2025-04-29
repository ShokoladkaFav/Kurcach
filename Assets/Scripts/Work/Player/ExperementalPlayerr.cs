using UnityEngine;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Health and Mana")]
    public float maxHealth = 560f;
    public float currentHealth;
    public float maxMana = 315f;
    public float currentMana;
    public float healthRegen = 2f;
    public float manaRegen = 1f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentMana = maxMana;

        // Викликаємо метод Spawned після спавну гравця
        Spawned();
    }

    public void Spawned()
    {
        // Запускаємо корутину для передачі компонентів в ShopManager
        StartCoroutine(AssignComponentsToShopManager());

        // Додатковий метод для перевірки дочірніх об'єктів
        LogChildObjects();

        Debug.Log("Гравець успішно спавнено!");
    }

    private void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        RegenerateHealthAndMana();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator AssignComponentsToShopManager()
    {
        MoneyDisplay moneyDisplay = null;
        Inventory inventory = null;
        int attempts = 0;
        const int maxAttempts = 10;

        while (attempts < maxAttempts)
        {
            // Знаходимо об'єкт інвентаря
            Transform inventoryTransform = transform.Find("INVENTORY");
            bool wasInactive = false;

            if (inventoryTransform != null && !inventoryTransform.gameObject.activeInHierarchy)
            {
                inventoryTransform.gameObject.SetActive(true);
                wasInactive = true;
            }

            moneyDisplay = GetComponentInChildren<MoneyDisplay>();
            inventory = GetComponentInChildren<Inventory>();

            if (moneyDisplay != null && inventory != null)
            {
                ShopManager.Instance.SetMoneyDisplay(moneyDisplay);
                ShopManager.Instance.SetInventory(inventory);
                Debug.Log("Компоненти MoneyDisplay та Inventory успішно передані в ShopManager.");

                // Повертаємо інвентар у початковий стан (неактивний)
                if (wasInactive)
                {
                    inventoryTransform.gameObject.SetActive(false);
                }

                yield break;
            }

            attempts++;
            Debug.Log($"Спроба {attempts}: Не вдалося знайти MoneyDisplay або Inventory. Очікуємо...");
            yield return new WaitForSeconds(1f);
        }

        Debug.LogError("Не вдалося знайти MoneyDisplay або Inventory після декількох спроб!");
    }

    private void LogChildObjects()
    {
        Debug.Log("Список дочірніх об'єктів цього гравця:");
        foreach (Transform child in transform)
        {
            Debug.Log("Дочірній об'єкт: " + child.name);
        }
    }

    private void RegenerateHealthAndMana()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegen * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        if (currentMana < maxMana)
        {
            currentMana += manaRegen * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, maxMana);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Debug.Log($"Гравець отримав {damage} урону. Здоров'я: {currentHealth}/{maxHealth}");
    }

    public void UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            Debug.Log($"Використано {amount} мани. Мана: {currentMana}/{maxMana}");
        }
        else
        {
            Debug.Log("Не вистачає мани!");
        }
    }
}
