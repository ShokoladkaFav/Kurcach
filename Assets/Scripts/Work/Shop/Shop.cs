using UnityEngine;

public class Shop : MonoBehaviour
{
    public Item[] shopItems; // Масив предметів, доступних у магазині

    private MoneyDisplay playerMoney;
    private Inventory inventory;

    void Start()
    {
        FindPlayer(); // Викликаємо метод для пошуку гравця
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player"); // Знаходимо гравця по тегу

        if (playerObj != null)
        {
            playerMoney = playerObj.GetComponent<MoneyDisplay>(); // Отримуємо скрипт грошей

            // Шукаємо Inventory серед дочірніх об'єктів вручну
            Transform inventoryTransform = playerObj.transform.Find("INVENTORY");

            if (inventoryTransform != null)
            {
                inventory = inventoryTransform.GetComponent<Inventory>();
            }
        }
    }

    public void TryBuy(int itemIndex)
    {
        if (playerMoney == null || inventory == null)
        {
            Debug.LogWarning("Повторний пошук гравця...");
            FindPlayer(); // Повторний пошук гравця, якщо його ще немає
        }

        if (playerMoney == null)
        {
            Debug.LogError("MoneyDisplay не знайдено у гравця!");
            return;
        }

        if (inventory == null)
        {
            Debug.LogError("Inventory не знайдено у гравця!");
            return;
        }

        if (itemIndex < 0 || itemIndex >= shopItems.Length)
        {
            Debug.LogError("Некоректний індекс товару!");
            return;
        }

        Item itemToBuy = shopItems[itemIndex];

        int copperCost = itemToBuy.costCopper;
        int silverCost = itemToBuy.costSilver;
        int goldCost = itemToBuy.costGold;

        int totalCost = copperCost + silverCost * 100 + goldCost * 10000;
        int totalMoney = playerMoney.GetTotalMoney();

        if (totalMoney >= totalCost)
        {
            if (playerMoney.DeductCoins(copperCost, silverCost, goldCost))
            {
                inventory.AddItem(itemToBuy);
                Debug.Log($"Гравець купив {itemToBuy.itemName}!");
            }
            else
            {
                Debug.LogError("Помилка при знятті грошей!");
            }
        }
        else
        {
            Debug.Log("Не вистачає грошей!");
        }
    }
}