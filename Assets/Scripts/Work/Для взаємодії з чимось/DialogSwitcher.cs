using UnityEngine;

public class DialogSwitcher : MonoBehaviour
{
    public GameObject currentDialogPanel;  // Поточний діалог (панель)
    public GameObject nextDialogPanel;     // Наступний діалог (панель)

    private bool isPlayerNearby = false;

    // Метод для перемикання між панелями діалогів
    public void SwitchToNextDialog()
    {
        currentDialogPanel.SetActive(false);  // Приховуємо поточний діалог
        nextDialogPanel.SetActive(true);      // Показуємо наступний діалог
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // Закрити всі вікна та скинути стан магазину
            currentDialogPanel.SetActive(false);
            nextDialogPanel.SetActive(false);
        }
    }
}
