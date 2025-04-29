using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public GameObject panelQuestsName;  // Panel_Quests_Name
    public GameObject panelQuestsInfo;  // Panel_Quests_Info
    public Transform questListContainer; // ��������� ��� ���� ������
    public GameObject questEntryPrefab; // ������ ������ ������

    public Text questTitleText;
    public Text questDescriptionText;
    public Text questRewardText;
    public Text questProgressText;

    public static QuestUIManager instance; // Singleton
    public Quest currentSelectedQuest; // �������� �������� �����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("QuestUIManager ������������!");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        panelQuestsInfo.SetActive(false); // ������ ������ ������� �� �����
        UpdateQuestList();
    }

    public void UpdateQuestList()
    {
        Debug.Log("��������� ������ ������...");
        // ������� ���� ��������
        foreach (Transform child in questListContainer)
        {
            Destroy(child.gameObject);
        }

        // ������ ������ ������
        foreach (Quest quest in QuestManager.instance.activeQuests)
        {
            Debug.Log($"������ ����� � UI: {quest.questName}");
            GameObject questEntry = Instantiate(questEntryPrefab, questListContainer);
            QuestEntryUI entryUI = questEntry.GetComponent<QuestEntryUI>();
            entryUI.SetQuestData(quest, this);
        }
    }

    public void UpdateQuestProgress(Quest quest)
    {
        if (currentSelectedQuest == quest)
        {
            questProgressText.text = $"�������: {quest.collectedItems} / {quest.totalItems}";
            Debug.Log($"��������� �������� ��� {quest.questName}: {quest.collectedItems}/{quest.totalItems}");
        }
    }

    public void ShowQuestInfo(Quest quest)
    {
        panelQuestsInfo.SetActive(true);
        questTitleText.text = quest.questName;
        questDescriptionText.text = quest.description;
        questRewardText.text = $"��������: {quest.copperReward} , {quest.silverReward} , {quest.goldReward} ";
        questProgressText.text = $"�������: {quest.collectedItems}/{quest.totalItems}";
        currentSelectedQuest = quest;
    }

    //  ��������� ������ ����������� ������ � Panel_Quests_Name
    public void RemoveQuestFromUI(Quest quest)
    {
        foreach (Transform child in questListContainer)
        {
            QuestEntryUI entry = child.GetComponent<QuestEntryUI>();
            if (entry != null && entry.GetQuest() == quest) //  ������������� GetQuest()
            {
                Destroy(child.gameObject);
                Debug.Log($"����� '{quest.questName}' �������� � UI");
                break;
            }
        }

        if (currentSelectedQuest == quest)
        {
            panelQuestsInfo.SetActive(false);
        }
    }
}
