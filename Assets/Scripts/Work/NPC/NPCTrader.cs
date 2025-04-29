using UnityEngine;

public class NPC_Trader : MonoBehaviour
{
    public GameObject dialogueUI; // Панель для діалогу
    public GameObject shopUI;     // Панель магазину

    private bool isPlayerNearby = false;
    private bool isShopOpen = false; // Змінна для відслідковування стану магазину

    void Update()
    {
        // Якщо гравець близько і натискає E, відкривається діалог (якщо магазин не відкритий)
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isShopOpen)
        {
            ToggleDialogue();
        }
    }

    private void ToggleDialogue()
    {
        // Перемикання стану діалогового вікна
        bool isActive = dialogueUI.activeSelf;
        dialogueUI.SetActive(!isActive);

        if (isActive)
        {
            Debug.Log("Діалог закрито");
        }
        else
        {
            Debug.Log("Діалог відкрито");
        }
    }

    public void OpenShop()
    {
        // Закрити діалог, якщо він відкритий
        dialogueUI.SetActive(false);
        Debug.Log("Діалог закрився");

        // Відкрити магазин
        shopUI.SetActive(true);
        isShopOpen = true; // Позначити, що магазин відкритий
        Debug.Log("Магазин відкрився");
    }

    public void CloseShop()
    {
        // Закрити магазин
        shopUI.SetActive(false);
        isShopOpen = false; // Позначити, що магазин закритий
        Debug.Log("Магазин закрився");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Гравець у зоні торговця");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // Закрити всі вікна та скинути стан магазину
            dialogueUI.SetActive(false);
            shopUI.SetActive(false);
            isShopOpen = false; // Скидаємо стан магазину
            Debug.Log("Гравець вийшов із зони торговця");
        }
    }
}
