using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string description;
    public List<string> requiredItems; // Які предмети треба принести

    // Нагорода
    public int copperReward;
    public int silverReward;
    public int goldReward;
    public string rewardItem; // Назва предмета-нагороди (може бути пустою)

    public bool isCompleted = false;
    public Action onQuestCompleted;

    public int totalItems { get; private set; } // Загальна кількість предметів
    public int collectedItems => totalItems - requiredItems.Count; // Кількість зібраних предметів

    public Quest(string name, string desc, List<string> items, int copper, int silver, int gold, string itemReward)
    {
        questName = name;
        description = desc;
        requiredItems = new List<string>(items);
        totalItems = requiredItems.Count; // Фіксуємо початкову кількість предметів
        copperReward = copper;
        silverReward = silver;
        goldReward = gold;
        rewardItem = itemReward;
        isCompleted = false;
    }

    public void CompleteQuest()
    {
        isCompleted = true;
        onQuestCompleted?.Invoke();
    }
}
