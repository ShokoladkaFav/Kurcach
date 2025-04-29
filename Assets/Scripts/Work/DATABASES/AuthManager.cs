using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;

    private string loginUrl = "http://localhost/Kursach/login.php";
    private string registerUrl = "http://localhost/Kursach/register.php";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnLoginButtonClick()
    {
        StartCoroutine(Login(usernameInput.text, passwordInput.text));
    }

    public void OnRegisterButtonClick()
    {
        StartCoroutine(Register(usernameInput.text, passwordInput.text));
    }

    IEnumerator Login(string username, string password)
    {
        string jsonData = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = new UnityWebRequest(loginUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            messageText.text = "Помилка сервера: " + www.error;
        }
        else
        {
            Debug.Log("Отриманий JSON: " + www.downloadHandler.text);
            try
            {
                ServerResponse response = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
                if (response.success)
                {
                    messageText.text = "Вхід успішний!";

                    // Збереження даних гравця
                    PlayerPrefs.SetInt("UserID", response.user_id);
                    PlayerPrefs.SetString("Username", response.username);
                    PlayerPrefs.SetInt("Level", response.level);
                    PlayerPrefs.Save();

                    Debug.Log("UserID: " + PlayerPrefs.GetInt("UserID"));
                    Debug.Log("Username: " + PlayerPrefs.GetString("Username"));
                    Debug.Log("Level: " + PlayerPrefs.GetInt("Level"));
                    Debug.Log("hasRaceSkill: " + response.hasRaceSkill);

                    if (response.hasRaceSkill)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("GameWorld");
                    }
                    else
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("GameRegister");
                    }
                }
                else
                {
                    messageText.text = "Помилка: " + response.message;
                }
            }
            catch
            {
                messageText.text = "Помилка обробки відповіді";
                Debug.LogError("Некоректний JSON від сервера: " + www.downloadHandler.text);
            }
        }
    }

    IEnumerator Register(string username, string password)
    {
        string jsonData = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = new UnityWebRequest(registerUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            messageText.text = "Помилка сервера: " + www.error;
        }
        else
        {
            Debug.Log("Отримана відповідь: " + www.downloadHandler.text);

            try
            {
                ServerResponse response = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
                messageText.text = response.message;
            }
            catch
            {
                messageText.text = "Невірний формат відповіді";
                Debug.LogError("Сервер повернув некоректний JSON: " + www.downloadHandler.text);
            }
        }
    }

    [System.Serializable]
    public class ServerResponse
    {
        public bool success;
        public string message;
        public int user_id;
        public int level;
        public string username;
        public bool hasRaceSkill;
    }
}
