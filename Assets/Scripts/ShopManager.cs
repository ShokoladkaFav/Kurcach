using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private MoneyDisplay moneyDisplay;
    private Inventory inventory;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetMoneyDisplay(MoneyDisplay display)
    {
        moneyDisplay = display;
        Debug.Log("MoneyDisplay успішно встановлено!");
    }

    public void SetInventory(Inventory inv)
    {
        inventory = inv;
        Debug.Log("Inventory успішно встановлено!");
    }

    private MoneyDisplay GetMoneyDisplay()
    {
        if (moneyDisplay == null)
            Debug.LogError("MoneyDisplay не встановлено!");
        return moneyDisplay;
    }

    private Inventory GetInventory()
    {
        if (inventory == null)
            Debug.LogError("Inventory не встановлено!");
        return inventory;
    }

    public void BuyItem(Item item)
    {
        var moneyDisplay = GetMoneyDisplay();
        var inventory = GetInventory();

        if (moneyDisplay == null || inventory == null)
        {
            Debug.LogError("Не вдалося здійснити покупку: відсутній інвентар або MoneyDisplay.");
            return;
        }

        int playerID = PlayerPrefs.GetInt("UserID", -1);
        if (playerID == -1)
        {
            Debug.LogError("PlayerID не знайдено в PlayerPrefs! Авторизуйся спочатку.");
            return;
        }

        int totalMoney = moneyDisplay.GetTotalMoney();
        int itemPrice = item.costCopper + item.costSilver * 100 + item.costGold * 10000;

        if (totalMoney < itemPrice)
        {
            Debug.LogWarning("Недостатньо монет для покупки!");
            return;
        }

        // Перевірка місця в інвентарі
        Item testItem = new Item
        {
            id = item.id,
            itemName = item.itemName,
            currentStackSize = 1,
            itemIcon = item.itemIcon,
            isStackable = item.isStackable,
            maxStackSize = item.maxStackSize
        };

        if (!inventory.CanAddItem(testItem))
        {
            Debug.LogWarning("Інвентар переповнений! Предмет не буде куплено.");
            return;
        }

        StartCoroutine(BuyItemRequest(item, playerID));
    }

    IEnumerator BuyItemRequest(Item originalItem, int playerID)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", playerID);
        form.AddField("item_id", originalItem.id);
        form.AddField("copper", originalItem.costCopper);
        form.AddField("silver", originalItem.costSilver);
        form.AddField("gold", originalItem.costGold);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/Kursach/buy_item.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Помилка покупки: " + www.error);
            yield break;
        }

        string result = www.downloadHandler.text;
        Debug.Log("Відповідь сервера: " + result);

        try
        {
            BuyItemResponse response = JsonUtility.FromJson<BuyItemResponse>(result);

            if (!response.success)
            {
                Debug.LogWarning("Не вдалося купити предмет: " + response.message);
                yield break;
            }

            Debug.Log("Предмет куплено успішно!");

            // Створюємо тимчасовий предмет для додавання
            Item tempItem = new Item
            {
                id = response.item_id,
                itemName = response.item_name,
                currentStackSize = 1,
                itemIcon = originalItem.itemIcon,
                isStackable = originalItem.isStackable,
                maxStackSize = originalItem.maxStackSize,
                costCopper = originalItem.costCopper,
                costSilver = originalItem.costSilver,
                costGold = originalItem.costGold
            };

            // Додаємо предмет в інвентар
            bool added = inventory.AddItem(tempItem);

            if (added)
            {
                // Знаходимо предмет, який тільки-но був доданий
                Item realItem = inventory.items.Find(i =>
                    i.id == tempItem.id &&
                    i.itemName == tempItem.itemName &&
                    i.itemIcon == tempItem.itemIcon &&
                    i.uiElement != null &&
                    i.currentStackSize == 1
                );

                if (realItem != null)
                {
                    Debug.Log($"[ShopManager] Предмет {realItem.itemName} (ID: {realItem.id}) додано з UI.");
                }
                else
                {
                    Debug.LogWarning("[ShopManager] Не вдалося знайти щойно доданий предмет у списку items.");
                }

                if (moneyDisplay.DeductCoins(originalItem.costCopper, originalItem.costSilver, originalItem.costGold))
                {
                    Debug.Log("Монети успішно знято.");
                    inventory.UpdateInventoryUI();
                    moneyDisplay.UpdateUI();
                }
                else
                {
                    Debug.LogError("Помилка при знятті монет!");
                }
            }
            else
            {
                Debug.LogWarning("Інвентар повний — предмет не додано! Монети не знято.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("[BuyItem] Помилка обробки JSON: " + e.Message);
        }
    }

    [System.Serializable]
    private class BuyItemResponse
    {
        public bool success;
        public string message;
        public int item_id;
        public string item_name;
    }
}
