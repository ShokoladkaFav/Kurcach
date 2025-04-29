using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Text usernameText;
    public Text levelText;

    void Start()
    {
        string username = PlayerPrefs.GetString("Username", "Гість");
        int level = PlayerPrefs.GetInt("Level", 1);

        usernameText.text = "Ім'я: " + username;
        levelText.text = "Рівень: " + level;
    }
}
