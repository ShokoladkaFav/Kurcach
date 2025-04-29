using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class RaceSkillManager : MonoBehaviour
{
    public Button[] raceButtons;  // Масив кнопок рас
    public Button[] skillButtons; // Масив кнопок навичок
    public Button confirmButton;  // Кнопка підтвердження

    private int selectedRaceId = -1;
    private int selectedSkillId = -1;

    private string saveRaceSkillUrl = "http://localhost/Kursach/save_race_skill.php";

    void Start()
    {
        // Прив'язуємо обробник до кнопок рас
        for (int i = 0; i < raceButtons.Length; i++)
        {
            int raceId = i + 1; // Припустимо, що ID раси йдуть від 1
            raceButtons[i].onClick.AddListener(() => SelectRace(raceId));
        }

        // Прив'язуємо обробник до кнопок навичок
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int skillId = i + 1; // Припустимо, що ID навичок йдуть від 1
            skillButtons[i].onClick.AddListener(() => SelectSkill(skillId));
        }

        confirmButton.onClick.AddListener(SaveRaceAndSkill);
    }

    public void SelectRace(int raceId)
    {
        selectedRaceId = raceId;
        Debug.Log("Обрана раса: " + selectedRaceId);
    }

    public void SelectSkill(int skillId)
    {
        selectedSkillId = skillId;
        Debug.Log("Обрана навичка: " + selectedSkillId);
    }

    public void SaveRaceAndSkill()
    {
        if (selectedRaceId == -1 || selectedSkillId == -1)
        {
            Debug.Log("Спочатку виберіть расу та навичку!");
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
                Debug.Log("Помилка сервера: " + www.error);
            }
            else
            {
                Debug.Log("Раса та навичка збережені!");
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameWorld");
            }
        }
    }
}
