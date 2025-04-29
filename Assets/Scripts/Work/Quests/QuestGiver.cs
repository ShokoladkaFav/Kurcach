using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public string questName = "������ ������";
    public string questDescription = "������� ��� 3 ������.";
    public List<string> requiredItems = new List<string> { "������", "������", "������" };

    // ��������: ������ �� �������
    public int copperReward = 10;
    public int silverReward = 5;
    public int goldReward = 1;
    public string rewardItem = "˳�������� ����"; // ���� ���� ��������, �������� ������� ""

    private bool questGiven = false;
    private Quest currentQuest;

    public GameObject questUI; // UI ��� ������ (������ � ��������)
    public Button acceptButton;
    public Button declineButton;
    public Button completeButton;

    // ������ ��'����, �� ������ ��������� ��� �������� ������
    public List<GameObject> questItemsInWorld;

    private void Start()
    {
        questUI.SetActive(false); // ������ UI �� �����
        acceptButton.onClick.AddListener(GiveQuest);
        declineButton.onClick.AddListener(DeclineQuest);
        completeButton.onClick.AddListener(TryCompleteQuest);
    }

    public void OpenQuestUI()
    {
        questUI.SetActive(true);
    }

    public void CloseQuestUI()
    {
        questUI.SetActive(false);
    }

    public void GiveQuest()
    {
        if (!questGiven)
        {
            Quest newQuest = new Quest(questName, questDescription, new List<string>(requiredItems), copperReward, silverReward, goldReward, rewardItem);
            QuestManager.instance.AddQuest(newQuest);
            questGiven = true;
            Debug.Log($"����� '{questName}' ��������!");

            // �������� �� ������� ��������
            foreach (GameObject item in questItemsInWorld)
            {
                if (item != null)
                {
                    item.SetActive(true);
                }
            }
        }
    }

    public void DeclineQuest()
    {
        Debug.Log("������� ������� �����.");
        CloseQuestUI();
    }

    public void TryCompleteQuest()
    {
        if (questGiven && currentQuest != null && currentQuest.isCompleted)
        {
            QuestManager.instance.CompleteQuest(currentQuest);
            Debug.Log($"����� '{questName}' �����!");
            questGiven = false;
            CloseQuestUI();
        }
        else
        {
            Debug.Log("����� �� �� ���������!");
        }
    }
}
