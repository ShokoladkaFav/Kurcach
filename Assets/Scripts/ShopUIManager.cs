using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject itemPrefab; // Префаб для UI-предмету
    public Transform itemContainer; // Контейнер, куди додаються предмети

    public void DisplayItems(List<Item> items)
    {
        // Очищаємо старі предмети перед оновленням
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        // Створюємо нові UI-елементи
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(itemPrefab, itemContainer);
            ItemUI itemUI = newItem.GetComponent<ItemUI>();
            if (itemUI != null)
            {
                itemUI.Setup(item); // Подаємо об'єкт Item
            }
        }
    }
}
