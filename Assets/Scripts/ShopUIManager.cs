using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject itemPrefab; // ������ ��� UI-��������
    public Transform itemContainer; // ���������, ���� ��������� ��������

    public void DisplayItems(List<Item> items)
    {
        // ������� ���� �������� ����� ����������
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        // ��������� ��� UI-��������
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(itemPrefab, itemContainer);
            ItemUI itemUI = newItem.GetComponent<ItemUI>();
            if (itemUI != null)
            {
                itemUI.Setup(item); // ������ ��'��� Item
            }
        }
    }
}
