using System.Collections.Generic;
using UnityEngine;

public class Shopp : MonoBehaviour
{
    public GameObject itemUIPrefab;
    public Transform contentPanel;

    public void LoadItemsToShop(List<Item> items)
    {
        foreach (Item item in items)
        {
            Debug.Log("Створюю UI для: " + item.itemName);

            GameObject itemUIObj = Instantiate(itemUIPrefab, contentPanel);
            ItemUI itemUI = itemUIObj.GetComponent<ItemUI>();

            if (itemUI != null)
            {
                itemUI.Setup(item);
            }
        }
    }
}
