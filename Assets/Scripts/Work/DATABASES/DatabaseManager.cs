using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    private string url = "http://localhost/Kursach/get_players.php";
    private string registerUrl = "http://localhost/Kursach/register.php";

    public static DatabaseManager Instance { get; private set; }

    public int PlayerID { get; set; }  // ������ ID ������
    public string PlayerUsername { get; set; }
    public int PlayerLevel { get; set; }  // ������ ����� ������

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("DatabaseManager ����������!");
    }

    public void RegisterUser(string username, string password)
    {
        StartCoroutine(RegisterRequest(username, password));
    }

    IEnumerator RegisterRequest(string username, string password)
    {
        string jsonData = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        Debug.Log("³���������� JSON: " + jsonData);

        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(registerUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("������� ������: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("�������� �������: " + responseText);

            try
            {
                RegistrationResponse response = JsonUtility.FromJson<RegistrationResponse>(responseText);
                if (response.success)
                {
                    Debug.Log("��������� ������!");
                    PlayerUsername = username;
                }
                else
                {
                    Debug.LogError("������� ���������: " + response.message);
                }
            }
            catch
            {
                Debug.LogError("������� ������ ������: " + responseText);
            }
        }
    }

    [System.Serializable]
    class RegistrationResponse
    {
        public bool success;
        public string message;
    }
}
