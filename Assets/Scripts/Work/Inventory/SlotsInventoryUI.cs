using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform slotsParent;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // ���� � itemName, �������� � ������ ��������
        Dictionary<string, List<Item>> groupedItems = new Dictionary<string, List<Item>>();

        foreach (Item item in inventory.items)
        {
            if (!groupedItems.ContainsKey(item.itemName))
            {
                groupedItems[item.itemName] = new List<Item>();
            }
            groupedItems[item.itemName].Add(item);
        }

        foreach (var kvp in groupedItems)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotsParent);
            Item firstItem = kvp.Value[0]; // ������ ������� ����� ����

            Image icon = newSlot.transform.Find("Icon").GetComponent<Image>();
            if (icon != null && firstItem.itemIcon != null)
            {
                icon.sprite = firstItem.itemIcon;
            }

            Text quantityText = newSlot.transform.Find("QuantityText").GetComponent<Text>();
            if (quantityText != null)
            {
                int totalCount = 0;
                foreach (var it in kvp.Value)
                {
                    totalCount += it.currentStackSize;
                }

                quantityText.text = totalCount > 1 ? totalCount.ToString() : "";
            }

            Text itemNameText = newSlot.transform.Find("ItemNameText").GetComponent<Text>();
            if (itemNameText != null)
            {
                itemNameText.text = firstItem.itemName.Length > 10
                    ? firstItem.itemName.Substring(0, 10) + "..."
                    : firstItem.itemName;
            }

            AddClickFunctionality(newSlot, kvp.Value); // �������� ������ �������� ���� �����
        }
    }

    private void AddClickFunctionality(GameObject slot, List<Item> items)
    {
        Item mainItem = items[0];

        EventTrigger eventTrigger = slot.AddComponent<EventTrigger>();

        // ˳��� ��� � ����������
        EventTrigger.Entry leftClickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        leftClickEntry.callback.AddListener((data) =>
        {
            PointerEventData pointerData = (PointerEventData)data;
            if (pointerData.button == PointerEventData.InputButton.Left)
            {
                EquipItem(mainItem);
            }
        });
        eventTrigger.triggers.Add(leftClickEntry);

        // ������ ��� � ��������� ��������� ������� ����� ��������
        EventTrigger.Entry rightClickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        rightClickEntry.callback.AddListener((data) =>
        {
            PointerEventData pointerData = (PointerEventData)data;
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log($"[SlotsInventoryUI] ��� �� ����� � ���������: {mainItem.itemName} (ID: {mainItem.id})");

                if (mainItem != null && mainItem.id > 0)
                {
                    //  ���������: ��������� �������� item � ��������
                    Item realItem = Inventory.instance.items.Find(i => i.id == mainItem.id);

                    if (realItem != null)
                    {
                        Inventory.instance.HandleItemRightClick(realItem);
                    }
                    else
                    {
                        Debug.LogWarning($"[SlotsInventoryUI] ������� {mainItem.itemName} (ID: {mainItem.id}) �� �������� � ��������.");
                    }
                }
                else
                {
                    Debug.LogWarning("[SlotsInventoryUI] ���� �������� �������� � ����.");
                }
            }
        });
        eventTrigger.triggers.Add(rightClickEntry);
    }

    private void EquipItem(Item item)
    {
        EquipmentSlot slot = inventory.equipmentSlots.Find(s => s.slotType == item.itemType);
        if (slot != null)
        {
            if (slot.currentItem != null)
            {
                if (!inventory.AddItem(slot.currentItem))
                {
                    Debug.Log("�������� ����������!");
                    return;
                }
            }

            slot.SetItem(item);
            inventory.RemoveItem(item);
            UpdateUI();
        }
        else
        {
            Debug.Log("���� ���������� ����� ��� ����� ��������.");
        }
    }
}
