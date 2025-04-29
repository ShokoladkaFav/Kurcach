using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();
    public InventoryUI inventoryUI;
    public int maxCapacity = 100;

    public Canvas notificationCanvas;
    public TextMeshProUGUI notificationText;
    public float notificationDuration = 2f;

    public TextMeshProUGUI inventoryCapacityText;

    public static Inventory instance;
    public Transform inventoryUIParent;
    public GameObject itemUIPrefab;

    private int userID;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("UserID"))
        {
            Debug.LogWarning("UserID не знайдено в PlayerPrefs! Перевірте збереження користувача.");
            return;
        }

        userID = PlayerPrefs.GetInt("UserID");
        StartCoroutine(LoadInventory());
    }

    private void Update()
    {
        HandleRightClickOnItem();
    }

    public void HandleItemRightClick(Item item)
    {
        StartCoroutine(DecreaseItemQuantityOnServer(item));
    }


    private void HandleRightClickOnItem()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Input.mousePosition;
            Debug.Log($"[HandleRightClickOnItem] ПКМ на позиції: {mousePos}");

            foreach (var item in items)
            {
                if (item.uiElement == null)
                {
                    Debug.LogWarning($"[HandleRightClickOnItem] {item.itemName} не має uiElement. Створюємо.");
                    CreateInventoryItemUI(item);

                    Debug.LogWarning($"[HandleRightClickOnItem] Створили uiElement для {item.itemName}. Треба зачекати наступний кадр.");
                    return; // Важливо! Повертаємось і не обробляємо цей клік в цьому кадрі
                }

                RectTransform rectTransform = item.uiElement?.GetComponent<RectTransform>();
                if (rectTransform == null) continue;

                bool inside = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos);
                Debug.Log($"[HandleRightClickOnItem] Перевіряємо: {item.itemName}, всередині: {inside}");

                if (inside)
                {
                    Debug.Log($"[HandleRightClickOnItem] Натиснуто ПКМ по: {item.itemName}");

                    // Тепер шукаємо InventoryItemUI скрипт із цього об'єкта
                    InventoryItemUI uiScript = item.uiElement.GetComponent<InventoryItemUI>();
                    if (uiScript != null && uiScript.linkedItem != null && uiScript.linkedItem.id > 0)
                    {
                        Inventory.instance.HandleItemRightClick(uiScript.linkedItem);
                    }
                    else
                    {
                        Debug.LogError($"[HandleRightClickOnItem] Проблема з linkedItem в UI для {item.itemName}");
                    }
                    break;
                }
            }
        }
    }


    private IEnumerator DecreaseItemQuantityOnServer(Item item)
    {
        if (item == null || item.id <= 0)
        {
            Debug.LogError("[DecreaseItemQuantityOnServer] Item дорівнює null або має некоректний ID.");
            yield break;
        }

        int updatedQuantity = item.currentStackSize - 1;

        string url;
        WWWForm form = new WWWForm();
        form.AddField("user_id", userID);
        form.AddField("item_id", item.id);

        if (updatedQuantity <= 0)
        {
            url = "http://localhost/Kursach/deleteItem.php";
        }
        else
        {
            url = "http://localhost/Kursach/update_item_quantity.php";
            form.AddField("quantity", updatedQuantity);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[DecreaseItemQuantityOnServer] ПОМИЛКА! " + www.error);
                yield break;
            }

            if (updatedQuantity <= 0)
            {
                Debug.Log($"[DecreaseItemQuantityOnServer] Видаляємо предмет {item.itemName} локально.");
                items.Remove(item);
                if (item.uiElement != null)
                    Destroy(item.uiElement);
            }
            else
            {
                Debug.Log($"[DecreaseItemQuantityOnServer] Зменшуємо кількість для {item.itemName} до {updatedQuantity}");
                item.currentStackSize = updatedQuantity;
                UpdateItemUI(item);
            }

            UpdateInventoryUI(); // <<< ОБОВ'ЯЗКОВО! Оновлюємо повністю UI
        }
    }

    public void UpdateInventoryUI()
    {
        inventoryUI?.UpdateUI();
        UpdateCapacityUI();
    }

    public void UpdateCapacityUI()
    {
        if (inventoryCapacityText != null)
        {
            inventoryCapacityText.text = $"{GetTotalItemCount()} / {maxCapacity}";
            Debug.Log($"[UpdateCapacityUI] Поточна місткість: {GetTotalItemCount()} / {maxCapacity}");
        }
    }

    public int GetTotalItemCount()
    {
        int total = 0;
        foreach (var item in items)
            total += item.currentStackSize;
        return total;
    }

    public bool CanAddItem(Item item)
    {
        int amountToAdd = item.currentStackSize;
        int freeCapacity = maxCapacity - GetTotalItemCount();

        foreach (Item existingItem in items)
        {
            if (existingItem.id == item.id && existingItem.isStackable && existingItem.currentStackSize < existingItem.maxStackSize)
            {
                int spaceLeft = existingItem.maxStackSize - existingItem.currentStackSize;
                int stackableAmount = Mathf.Min(spaceLeft, amountToAdd);
                amountToAdd -= stackableAmount;

                if (amountToAdd <= 0)
                    return true;
            }
        }

        while (amountToAdd > 0 && freeCapacity > 0)
        {
            int stackSize = Mathf.Min(item.maxStackSize, amountToAdd);
            amountToAdd -= stackSize;
            freeCapacity -= stackSize;

            if (amountToAdd <= 0)
                return true;
        }

        return false;
    }

    public bool AddItem(Item item)
    {
        if (item == null || item.id <= 0)
        {
            Debug.LogError($"[AddItem] Некоректний ID предмета або item = null. item.id = {item?.id}");
            return false;
        }

        // Шукаємо чи є вже такий предмет
        Item existingItem = items.Find(i => i.id == item.id);

        if (existingItem != null && existingItem.isStackable)
        {
            // Якщо є такий предмет — збільшуємо кількість
            int spaceLeft = existingItem.maxStackSize - existingItem.currentStackSize;
            int amountToAdd = Mathf.Min(spaceLeft, item.currentStackSize);

            existingItem.currentStackSize += amountToAdd;
            UpdateItemUI(existingItem);

            Debug.Log($"[AddItem] Додали {amountToAdd} до стека {existingItem.itemName}. Тепер {existingItem.currentStackSize} штук.");
        }
        else
        {
            // Якщо нема такого — додаємо як новий предмет
            Item newItem = new Item
            {
                id = item.id,
                itemName = item.itemName,
                itemIcon = item.itemIcon,
                isStackable = item.isStackable,
                maxStackSize = item.maxStackSize,
                currentStackSize = item.currentStackSize,
                isQuestItem = item.isQuestItem
            };

            items.Add(newItem);
            CreateInventoryItemUI(newItem);

            Debug.Log($"[AddItem] Додано новий предмет: {newItem.itemName} (ID: {newItem.id}), кількість: {newItem.currentStackSize}");
        }

        UpdateInventoryUI();
        return true;
    }


    public void RemoveItem(Item item)
    {
        items.Remove(item);
        if (item.uiElement != null)
            Destroy(item.uiElement);

        UpdateInventoryUI();
    }

    public void DecreaseSingleItem(Item item)
    {
        if (item == null) return;

        item.currentStackSize--;

        Debug.Log($"[DecreaseSingleItem] {item.itemName} залишилось: {item.currentStackSize}");

        if (item.currentStackSize <= 0)
        {
            RemoveItem(item);
        }
        else
        {
            UpdateItemUI(item);
            UpdateCapacityUI();
        }
    }

    private void ShowInventoryFullNotification()
    {
        if (notificationCanvas && notificationText)
        {
            notificationCanvas.gameObject.SetActive(true);
            notificationText.text = "Інвентар заповнений!";
            Invoke(nameof(HideNotification), notificationDuration);
        }
    }

    private void HideNotification()
    {
        if (notificationCanvas)
            notificationCanvas.gameObject.SetActive(false);
    }

    private IEnumerator LoadInventory()
    {
        string url = "http://localhost/Kursach/get_inventory.php";
        WWWForm form = new WWWForm();
        form.AddField("user_id", userID);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Помилка завантаження інвентаря: " + www.error);
                yield break;
            }

            string jsonResponse = www.downloadHandler.text;

            if (string.IsNullOrEmpty(jsonResponse))
            {
                Debug.LogError("Отримано порожню або некоректну відповідь.");
                yield break;
            }

            Debug.Log("[LoadInventory] JSON відповідь: " + jsonResponse);

            try
            {
                InventoryResponse response = JsonUtility.FromJson<InventoryResponse>(jsonResponse);

                if (response == null || !response.success)
                {
                    Debug.LogError("Сервер повідомив про помилку при обробці інвентаря.");
                    yield break;
                }

                foreach (Transform child in inventoryUIParent)
                {
                    DestroyImmediate(child.gameObject);
                }

                items.Clear();

                foreach (var itemData in response.items)
                {
                    Debug.Log($"[LoadInventory] item_id: {itemData.item_id}, name: {itemData.item_name}, quantity: {itemData.quantity}");

                    if (!int.TryParse(itemData.item_id, out int itemId) || itemId <= 0)
                    {
                        Debug.LogError($"[LoadInventory] Некоректне значення item_id: {itemData.item_id}");
                        continue;
                    }

                    if (!int.TryParse(itemData.quantity, out int quantity))
                    {
                        Debug.LogError($"[LoadInventory] Некоректне значення quantity: {itemData.quantity}");
                        continue;
                    }

                    Sprite icon = LoadIcon(itemData.icon_path);

                    Item newItem = new Item
                    {
                        id = itemId,
                        itemName = itemData.item_name,
                        currentStackSize = quantity,
                        itemIcon = icon,
                        isStackable = true,
                        maxStackSize = 99
                    };

                    Debug.Log($"[LoadInventory] Додається item: {newItem.itemName} (ID: {newItem.id})");

                    AddItem(newItem);
                }

                UpdateInventoryUI();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Помилка парсингу JSON: " + e.Message);
            }
        }
    }

    private Sprite LoadIcon(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath))
            return Resources.Load<Sprite>("icons/default");

        string cleanedPath = System.IO.Path.Combine("icons", iconPath.Replace(".png", ""));
        Sprite icon = Resources.Load<Sprite>(cleanedPath);

        if (icon == null)
        {
            Debug.LogWarning($"Іконку {cleanedPath} не знайдено. Завантажується стандартна.");
            icon = Resources.Load<Sprite>("icons/default");
        }

        return icon;
    }

    private void CreateInventoryItemUI(Item item)
    {
        if (!itemUIPrefab || !inventoryUIParent)
        {
            Debug.LogError("[CreateInventoryItemUI] Missing prefab or parent!");
            return;
        }

        GameObject newItemUI = Instantiate(itemUIPrefab, inventoryUIParent);
        item.uiElement = newItemUI;

        InventoryItemUI uiScript = newItemUI.GetComponent<InventoryItemUI>();
        if (uiScript == null)
        {
            Debug.LogError("[CreateInventoryItemUI] Префаб не має InventoryItemUI! Додай його через Інспектор!");
            return;
        }

        uiScript.linkedItem = item; // Прив'язуємо предмет

        // Оновлюємо UI іконки, тексту та кількості
        UnityEngine.UI.Image icon = newItemUI.transform.Find("Icon")?.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Text nameText = newItemUI.transform.Find("ItemNameText")?.GetComponent<UnityEngine.UI.Text>();
        UnityEngine.UI.Text quantityText = newItemUI.transform.Find("QuantityText")?.GetComponent<UnityEngine.UI.Text>();

        if (icon != null) icon.sprite = item.itemIcon;
        if (nameText != null) nameText.text = item.itemName;
        if (quantityText != null) quantityText.text = item.currentStackSize > 1 ? item.currentStackSize.ToString() : "";

        Debug.Log($"[CreateInventoryItemUI] Створено UI для предмета: {item.itemName} (ID: {item.id})");
        Debug.Log($"[CreateInventoryItemUI] Прив'язка: {item.itemName} (ID: {item.id}), linkedItem у UI: {uiScript.linkedItem?.itemName} (ID: {uiScript.linkedItem?.id})");

    }

    private void UpdateItemUI(Item item)
    {
        if (item.uiElement == null) return;

        var quantityText = item.uiElement.transform.Find("QuantityText")?.GetComponent<UnityEngine.UI.Text>();
        if (quantityText != null)
        {
            quantityText.text = item.currentStackSize > 1 ? item.currentStackSize.ToString() : "";
        }
    }

    public bool HasItem(Item item)
    {
        return items.Exists(i => i.id == item.id);
    }

    public int GetItemCount(Item targetItem)
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item.id == targetItem.id)
            {
                count += item.currentStackSize;
            }
        }
        return count;
    }

    [System.Serializable]
    private class InventoryResponse
    {
        public bool success;
        public string message;
        public List<ItemData> items;
    }

    [System.Serializable]
    private class ItemData
    {
        public string item_id;
        public string item_name;
        public string quantity;
        public string icon_path;
    }
}
