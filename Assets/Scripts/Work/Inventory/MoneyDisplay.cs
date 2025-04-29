using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class MoneyDisplay : MonoBehaviour
{
    public static MoneyDisplay Instance { get; private set; }

    public Text copperText;
    public Text silverText;
    public Text goldText;

    private int copperCoins;
    private int silverCoins;
    private int goldCoins;

    private string saveMoneyUrl = "http://localhost/Kursach/saveMoney.php";
    private string loadMoneyUrl = "http://localhost/Kursach/loadMoney.php";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static MoneyDisplay GetInstance()
    {
        if (Instance == null)
        {
            MoneyDisplay found = FindObjectOfType<MoneyDisplay>();
            if (found != null)
            {
                Instance = found;
                Debug.Log("MoneyDisplay знайдено на сцені через FindObjectOfType.");
            }
            else
            {
                GameObject playerObject = GameObject.Find("Experement Player(Clone)");
                if (playerObject != null)
                {
                    Instance = playerObject.GetComponent<MoneyDisplay>();
                    Debug.Log("MoneyDisplay знайдено через Experement Player(Clone).");
                }
            }
        }
        return Instance;
    }

    private void Start()
    {
        LogPlayerID();
        StartCoroutine(LoadMoneyCoroutine());
    }

    private void LogPlayerID()
    {
        int playerID = PlayerPrefs.GetInt("UserID", -1);
        if (playerID != -1)
        {
            Debug.Log("Player ID: " + playerID);
        }
        else
        {
            Debug.LogWarning("Player ID not found!");
        }
    }

    public int GetTotalMoney()
    {
        return copperCoins + (silverCoins * 100) + (goldCoins * 10000);
    }

    public void AddCoins(int copper, int silver, int gold)
    {
        copperCoins += copper;
        silverCoins += silver;
        goldCoins += gold;
        NormalizeCurrency();
        UpdateUI();
        StartCoroutine(SaveMoneyCoroutine());
    }

    public bool DeductCoins(int copper, int silver, int gold)
    {
        int totalCost = copper + (silver * 100) + (gold * 10000);
        if (GetTotalMoney() >= totalCost)
        {
            int totalMoney = GetTotalMoney() - totalCost;
            goldCoins = totalMoney / 10000;
            silverCoins = (totalMoney % 10000) / 100;
            copperCoins = totalMoney % 100;
            UpdateUI();
            StartCoroutine(SaveMoneyCoroutine());
            return true;
        }
        return false;
    }

    private void NormalizeCurrency()
    {
        silverCoins += copperCoins / 100;
        copperCoins %= 100;
        goldCoins += silverCoins / 100;
        silverCoins %= 100;
    }

    public void UpdateUI()
    {
        if (copperText != null) copperText.text = copperCoins.ToString();
        if (silverText != null) silverText.text = silverCoins.ToString();
        if (goldText != null) goldText.text = goldCoins.ToString();
    }

    private IEnumerator SaveMoneyCoroutine()
    {
        int playerID = PlayerPrefs.GetInt("UserID", -1);
        if (playerID == -1) yield break;

        WWWForm form = new WWWForm();
        form.AddField("user_id", playerID);
        form.AddField("copper", copperCoins);
        form.AddField("silver", silverCoins);
        form.AddField("gold", goldCoins);

        using (UnityWebRequest www = UnityWebRequest.Post(saveMoneyUrl, form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(" Монети успішно збережено!");
            }
            else
            {
                Debug.LogError(" Помилка збереження монет: " + www.error);
            }
        }
    }

    private IEnumerator LoadMoneyCoroutine()
    {
        int playerID = PlayerPrefs.GetInt("UserID", -1);
        if (playerID == -1) yield break;

        WWWForm form = new WWWForm();
        form.AddField("user_id", playerID);

        using (UnityWebRequest www = UnityWebRequest.Post(loadMoneyUrl, form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                if (!string.IsNullOrEmpty(json) && json.StartsWith("{") && json.EndsWith("}"))
                {
                    try
                    {
                        MoneyData data = JsonUtility.FromJson<MoneyData>(json);
                        copperCoins = data.copper;
                        silverCoins = data.silver;
                        goldCoins = data.gold;
                        UpdateUI();
                        Debug.Log(" Монети завантажено з бази даних!");
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(" Помилка парсингу JSON: " + ex.Message + "\nJSON: " + json);
                    }
                }
                else
                {
                    Debug.LogError(" Некоректний формат JSON: " + json);
                }
            }
            else
            {
                Debug.LogError(" Помилка завантаження монет: " + www.error);
            }
        }
    }

    [System.Serializable]
    public class MoneyData
    {
        public int copper;
        public int silver;
        public int gold;
    }
}
