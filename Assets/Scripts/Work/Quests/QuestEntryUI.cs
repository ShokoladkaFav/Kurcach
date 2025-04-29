using UnityEngine;
using UnityEngine.UI;

public class QuestEntryUI : MonoBehaviour
{
    public Text questNameText;
    private Quest quest;
    private QuestUIManager questUIManager;

    public Button questButton;  // Кнопка

    public void SetQuestData(Quest quest, QuestUIManager uiManager)
    {
        this.quest = quest;
        this.questUIManager = uiManager;
        questNameText.text = quest.questName;

        // Додаємо кнопку, щоб відкривати деталі квесту
        GetComponent<Button>().onClick.AddListener(() => questUIManager.ShowQuestInfo(quest));
    }

    public void Setup(Quest quest)
    {
        this.quest = quest; //  Обов’язково зберігаємо посилання на квест
        questNameText.text = quest.questName;
        questButton.onClick.RemoveAllListeners(); // Видаляємо старі лістенери
        questButton.onClick.AddListener(() =>
        {
            QuestUIManager.instance.ShowQuestInfo(quest);
            QuestUIManager.instance.currentSelectedQuest = quest;
        });
    }

    //  Додаємо гетер для доступу до квесту
    public Quest GetQuest()
    {
        return quest;
    }
}
