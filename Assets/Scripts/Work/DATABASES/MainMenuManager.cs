using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Text usernameText;
    public Text levelText;

    void Start()
    {
        string username = PlayerPrefs.GetString("Username", "ó���");
        int level = PlayerPrefs.GetInt("Level", 1);

        usernameText.text = "��'�: " + username;
        levelText.text = "г����: " + level;
    }
}
