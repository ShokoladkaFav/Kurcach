using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class RaceSkillManager : MonoBehaviour
{
    public Button[] raceButtons;  // ����� ������ ���
    public Button[] skillButtons; // ����� ������ �������
    public Button confirmButton;  // ������ ������������

    private int selectedRaceId = -1;
    private int selectedSkillId = -1;

    private string saveRaceSkillUrl = "http://localhost/Kursach/save_race_skill.php";

    void Start()
    {
        // ����'����� �������� �� ������ ���
        for (int i = 0; i < raceButtons.Length; i++)
        {
            int raceId = i + 1; // ����������, �� ID ���� ����� �� 1
            raceButtons[i].onClick.AddListener(() => SelectRace(raceId));
        }

        // ����'����� �������� �� ������ �������
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int skillId = i + 1; // ����������, �� ID ������� ����� �� 1
            skillButtons[i].onClick.AddListener(() => SelectSkill(skillId));
        }

        confirmButton.onClick.AddListener(SaveRaceAndSkill);
    }

    public void SelectRace(int raceId)
    {
        selectedRaceId = raceId;
        Debug.Log("������ ����: " + selectedRaceId);
    }

    public void SelectSkill(int skillId)
    {
        selectedSkillId = skillId;
        Debug.Log("������ �������: " + selectedSkillId);
    }

    public void SaveRaceAndSkill()
    {
        if (selectedRaceId == -1 || selectedSkillId == -1)
        {
            Debug.Log("�������� ������� ���� �� �������!");
            return;
        }

        StartCoroutine(SendRaceSkillToServer(selectedRaceId, selectedSkillId));
    }

    IEnumerator SendRaceSkillToServer(int raceId, int skillId)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", PlayerPrefs.GetString("Username"));
        form.AddField("race_id", raceId);
        form.AddField("skill_id", skillId);

        using (UnityWebRequest www = UnityWebRequest.Post(saveRaceSkillUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("������� �������: " + www.error);
            }
            else
            {
                Debug.Log("���� �� ������� ��������!");
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameWorld");
            }
        }
    }
}
