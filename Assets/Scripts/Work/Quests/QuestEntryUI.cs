using UnityEngine;
using UnityEngine.UI;

public class QuestEntryUI : MonoBehaviour
{
    public Text questNameText;
    private Quest quest;
    private QuestUIManager questUIManager;

    public Button questButton;  // ������

    public void SetQuestData(Quest quest, QuestUIManager uiManager)
    {
        this.quest = quest;
        this.questUIManager = uiManager;
        questNameText.text = quest.questName;

        // ������ ������, ��� ��������� ����� ������
        GetComponent<Button>().onClick.AddListener(() => questUIManager.ShowQuestInfo(quest));
    }

    public void Setup(Quest quest)
    {
        this.quest = quest; //  ���������� �������� ��������� �� �����
        questNameText.text = quest.questName;
        questButton.onClick.RemoveAllListeners(); // ��������� ���� ��������
        questButton.onClick.AddListener(() =>
        {
            QuestUIManager.instance.ShowQuestInfo(quest);
            QuestUIManager.instance.currentSelectedQuest = quest;
        });
    }

    //  ������ ����� ��� ������� �� ������
    public Quest GetQuest()
    {
        return quest;
    }
}
