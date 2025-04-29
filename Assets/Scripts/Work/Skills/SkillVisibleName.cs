using UnityEngine;

public class SkillSelectorVisible : MonoBehaviour
{
    [System.Serializable]
    public class Race
    {
        public string name; // ����� ����
        public GameObject sprite; // ������
    }

    public Race[] skills; // ����� ��� ��������� �������
    private GameObject currentModels; // ������� ������ ������

    // ����� ��� ������ �������
    public void SelectSkillsNameVisible(int index)
    {
        // ������ ������� ������, ���� ���� ����
        if (currentModels != null)
        {
            currentModels.SetActive(false);
        }

        // ������� ������� ������
        if (index >= 0 && index < skills.Length)
        {
            currentModels = skills[index].sprite;
            currentModels.SetActive(true);
        }
    }
}
