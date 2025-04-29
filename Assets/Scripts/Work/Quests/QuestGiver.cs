using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public string questName = "«найди €блука";
    public string questDescription = "ѕринеси мен≥ 3 €блука.";
    public List<string> requiredItems = new List<string> { "яблуко", "яблуко", "яблуко" };

    // Ќагорода: монети та предмет
    public int copperReward = 10;
    public int silverReward = 5;
    public int goldReward = 1;
    public string rewardItem = "Ћ≥кувальне з≥лл€"; // якщо немаЇ предмета, залишити порожн≥м ""

    private bool questGiven = false;
    private Quest currentQuest;

    public GameObject questUI; // UI дл€ квесту (панель з кнопками)
    public Button acceptButton;
    public Button declineButton;
    public Button completeButton;

    // —писок об'Їкт≥в, €к≥ будуть активован≥ при прийн€тт≥ квесту
    public List<GameObject> questItemsInWorld;

    private void Start()
    {
        questUI.SetActive(false); // ’оваЇмо UI на старт≥
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
            Debug.Log($" вест '{questName}' отримано!");

            // јктивуЇмо вс≥ квестов≥ предмети
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
        Debug.Log("√равець в≥дхилив квест.");
        CloseQuestUI();
    }

    public void TryCompleteQuest()
    {
        if (questGiven && currentQuest != null && currentQuest.isCompleted)
        {
            QuestManager.instance.CompleteQuest(currentQuest);
            Debug.Log($" вест '{questName}' здано!");
            questGiven = false;
            CloseQuestUI();
        }
        else
        {
            Debug.Log(" вест ще не завершено!");
        }
    }
}
