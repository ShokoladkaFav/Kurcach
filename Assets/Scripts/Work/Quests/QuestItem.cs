using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string itemName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager.instance.CheckQuestCompletion(itemName);
            Destroy(gameObject); // Видаляємо предмет після збору
        }
    }
}
