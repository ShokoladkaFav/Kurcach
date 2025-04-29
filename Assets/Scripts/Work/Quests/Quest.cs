using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string description;
    public List<string> requiredItems; // �� �������� ����� ��������

    // ��������
    public int copperReward;
    public int silverReward;
    public int goldReward;
    public string rewardItem; // ����� ��������-�������� (���� ���� ������)

    public bool isCompleted = false;
    public Action onQuestCompleted;

    public int totalItems { get; private set; } // �������� ������� ��������
    public int collectedItems => totalItems - requiredItems.Count; // ʳ������ ������� ��������

    public Quest(string name, string desc, List<string> items, int copper, int silver, int gold, string itemReward)
    {
        questName = name;
        description = desc;
        requiredItems = new List<string>(items);
        totalItems = requiredItems.Count; // Գ����� ��������� ������� ��������
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
