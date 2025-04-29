using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> activeQuests = new List<Quest>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        Debug.Log("QuestManager ініціалізовано!");
    }

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest);
        Debug.Log($"Квест {quest.questName} додано! Всього квестів: {activeQuests.Count}");
        QuestUIManager.instance.UpdateQuestList();
    }

    public void CheckQuestCompletion(string itemName)
    {
        foreach (Quest quest in activeQuests)
        {
            if (!quest.isCompleted && quest.requiredItems.Contains(itemName))
            {
                quest.requiredItems.Remove(itemName);

                QuestUIManager.instance.UpdateQuestProgress(quest);

                if (quest.requiredItems.Count == 0)
                {
                    CompleteQuest(quest);
                }
                break;
            }
        }
    }

    public void CompleteQuest(Quest quest)
    {
        quest.CompleteQuest();
        Debug.Log($"Квест '{quest.questName}' завершено!");

        // Видаляємо кнопку завершеного квесту
        QuestUIManager.instance.RemoveQuestFromUI(quest);

        // Отримуємо посилання на MoneyDisplay гравця
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            MoneyDisplay moneyDisplay = player.GetComponent<MoneyDisplay>();
            if (moneyDisplay != null)
            {
                // Передаємо правильні аргументи (золото, срібло, мідь)
                moneyDisplay.AddCoins(quest.goldReward, quest.silverReward, quest.copperReward);
            }
            else
            {
                Debug.LogError("MoneyDisplay не знайдено на гравцеві!");
            }
        }
        else
        {
            Debug.LogError("Гравця не знайдено!");
        }

        // Видача предмета, якщо він є
        if (!string.IsNullOrEmpty(quest.rewardItem))
        {
            Item rewardItem = new Item
            {
                itemName = quest.rewardItem,
                isQuestItem = false,
                costCopper = 0,
                costSilver = 0,
                costGold = 0,
                isStackable = true,
                maxStackSize = 1,
                currentStackSize = 1,
                itemType = Item.ItemType.None
            };

            bool added = Inventory.instance.AddItem(rewardItem);
            if (!added)
            {
                Debug.LogWarning("Інвентар заповнений! Нагороду не додано.");
            }
        }
    }
}
