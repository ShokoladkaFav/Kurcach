using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public GameObject panelQuestsName;  // Panel_Quests_Name
    public GameObject panelQuestsInfo;  // Panel_Quests_Info
    public Transform questListContainer; // Контейнер для назв квестів
    public GameObject questEntryPrefab; // Префаб кнопки квесту

    public Text questTitleText;
    public Text questDescriptionText;
    public Text questRewardText;
    public Text questProgressText;

    public static QuestUIManager instance; // Singleton
    public Quest currentSelectedQuest; // Поточний вибраний квест

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("QuestUIManager ініціалізовано!");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        panelQuestsInfo.SetActive(false); // Ховаємо панель деталей на старті
        UpdateQuestList();
    }

    public void UpdateQuestList()
    {
        Debug.Log("Оновлення списку квестів...");
        // Очищаємо старі елементи
        foreach (Transform child in questListContainer)
        {
            Destroy(child.gameObject);
        }

        // Додаємо активні квести
        foreach (Quest quest in QuestManager.instance.activeQuests)
        {
            Debug.Log($"Додаємо квест в UI: {quest.questName}");
            GameObject questEntry = Instantiate(questEntryPrefab, questListContainer);
            QuestEntryUI entryUI = questEntry.GetComponent<QuestEntryUI>();
            entryUI.SetQuestData(quest, this);
        }
    }

    public void UpdateQuestProgress(Quest quest)
    {
        if (currentSelectedQuest == quest)
        {
            questProgressText.text = $"Прогрес: {quest.collectedItems} / {quest.totalItems}";
            Debug.Log($"Оновлення прогресу для {quest.questName}: {quest.collectedItems}/{quest.totalItems}");
        }
    }

    public void ShowQuestInfo(Quest quest)
    {
        panelQuestsInfo.SetActive(true);
        questTitleText.text = quest.questName;
        questDescriptionText.text = quest.description;
        questRewardText.text = $"Нагорода: {quest.copperReward} , {quest.silverReward} , {quest.goldReward} ";
        questProgressText.text = $"Прогрес: {quest.collectedItems}/{quest.totalItems}";
        currentSelectedQuest = quest;
    }

    //  Видаляємо кнопку завершеного квесту з Panel_Quests_Name
    public void RemoveQuestFromUI(Quest quest)
    {
        foreach (Transform child in questListContainer)
        {
            QuestEntryUI entry = child.GetComponent<QuestEntryUI>();
            if (entry != null && entry.GetQuest() == quest) //  Використовуємо GetQuest()
            {
                Destroy(child.gameObject);
                Debug.Log($"Квест '{quest.questName}' видалено з UI");
                break;
            }
        }

        if (currentSelectedQuest == quest)
        {
            panelQuestsInfo.SetActive(false);
        }
    }
}
