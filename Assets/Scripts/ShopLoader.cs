using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopLoader : MonoBehaviour
{
    [System.Serializable]
    public class ShopData
    {
        public ShopItem[] items;
    }

    [System.Serializable]
    public class ShopItem
    {
        public int id;
        public string item_name;
        public int price_bronze;
        public int price_silver;
        public int price_gold;
        public string item_icon; // без .png
        public string item_type;
    }

    [Header("ID предметів, які будуть у магазині")]
    public List<int> allowedItemIDs = new List<int>();

    [Header("Префаб картки предмета в UI")]
    public GameObject itemUIPrefab;

    [Header("Контейнер для UI-предметів")]
    public Transform shopContentParent;

    private string url = "http://localhost/Kursach/get_shop_data.php";

    void Start()
    {
        StartCoroutine(LoadShopData());
    }

    IEnumerator LoadShopData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(" Помилка завантаження shop data: " + www.error);
            yield break;
        }

        string jsonText = www.downloadHandler.text.Trim();
        Debug.Log(" Отриманий JSON:\n" + jsonText);

        if (string.IsNullOrEmpty(jsonText) || !jsonText.Contains("items"))
        {
            Debug.LogError(" JSON пустий або некоректний!");
            yield break;
        }

        ShopData shopData;
        try
        {
            shopData = JsonUtility.FromJson<ShopData>(jsonText);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(" Помилка парсингу JSON: " + ex.Message);
            yield break;
        }

        if (shopData.items == null || shopData.items.Length == 0)
        {
            Debug.LogWarning(" Жодного предмета не знайдено в JSON.");
            yield break;
        }

        List<Item> loadedItems = new List<Item>();

        foreach (ShopItem si in shopData.items)
        {
            if (allowedItemIDs.Count > 0 && !allowedItemIDs.Contains(si.id))
                continue;

            //  ТУТ правильний шлях до іконки
            string cleanedIconPath = si.item_icon.Replace(".png", "");
            string iconPath = $"icons/{cleanedIconPath}";
            Debug.Log($" Спроба завантажити іконку за шляхом: Resources/{iconPath}");

            Sprite loadedSprite = Resources.Load<Sprite>(iconPath);

            if (loadedSprite == null)
            {
                Debug.LogWarning($" Іконка НЕ знайдена! Перевір шлях: Resources/{iconPath}");
            }
            else
            {
                Debug.Log($" Іконка завантажена: {iconPath}");
            }

            Item item = new Item
            {
                id = si.id,
                itemName = si.item_name,
                costCopper = si.price_bronze,
                costSilver = si.price_silver,
                costGold = si.price_gold,
                itemIcon = loadedSprite,
                isStackable = false,
                isQuestItem = false
            };

            if (System.Enum.TryParse(si.item_type.Replace("-", "_"), out Item.ItemType parsedType))
            {
                item.itemType = parsedType;
            }
            else
            {
                item.itemType = Item.ItemType.None;
                Debug.LogWarning($" Невідомий тип предмета: {si.item_type} → None");
            }

            loadedItems.Add(item);

            // 🖼 Створення UI
            if (itemUIPrefab != null && shopContentParent != null)
            {
                GameObject uiItem = Instantiate(itemUIPrefab, shopContentParent);

                Image iconImage = uiItem.transform.Find("Icon")?.GetComponent<Image>();
                Text nameText = uiItem.transform.Find("Name")?.GetComponent<Text>();

                if (iconImage != null) iconImage.sprite = loadedSprite;
                if (nameText != null) nameText.text = item.itemName;
            }
        }

        // ⬇ Передати товари в скрипт Shopp
        Shopp shopp = FindObjectOfType<Shopp>();
        if (shopp != null)
        {
            shopp.LoadItemsToShop(loadedItems);
        }
        else
        {
            Debug.LogWarning(" Shopp скрипт не знайдено на сцені!");
        }
    }
}
