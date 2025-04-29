using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class InventoryLoader : MonoBehaviour
{
    private string inventoryUrl = "http://localhost/Kursach/get_inventory.php";

    void Start()
    {
        int userID = PlayerPrefs.GetInt("UserID");
        StartCoroutine(LoadInventory(userID));
    }

    IEnumerator LoadInventory(int userID)
    {
        string jsonData = "{\"user_id\":" + userID + "}";
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = new UnityWebRequest(inventoryUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Помилка завантаження інвентаря: " + www.error);
        }
        else
        {
            Debug.Log("Інвентар JSON: " + www.downloadHandler.text);
            InventoryResponse response = JsonUtility.FromJson<InventoryResponse>(www.downloadHandler.text);

            if (response.success)
            {
                foreach (Item item in response.items)
                {
                    // Тут можна додати предмет до інвентарю у грі
                    Debug.Log("Предмет: " + item.item_name + " x" + item.quantity);
                    // Наприклад: InventoryManager.Instance.AddItem(item.item_id, item.quantity);
                }
            }
        }
    }

    [System.Serializable]
    public class InventoryResponse
    {
        public bool success;
        public List<Item> items;
    }

    [System.Serializable]
    public class Item
    {
        public int item_id;
        public string item_name;
        public int quantity;
        public string icon_path;
    }
}
