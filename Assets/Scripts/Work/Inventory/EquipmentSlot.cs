using UnityEngine;
using UnityEngine.UI;
using static Item;

public class EquipmentSlot : MonoBehaviour
{
    public ItemType slotType; // ��� ����� (�����, ��������� ����)
    public Item currentItem; // �������� ������� � ����
    public Inventory inventory; // ��������� �� ��������

    public void SetItem(Item item)
    {
        currentItem = item;
        UpdateSlotUI();
    }

    public void OnSlotClick()
    {
        if (currentItem != null)
        {
            if (inventory.AddItem(currentItem))
            {
                currentItem = null; // ��������� ������� � �����
                UpdateSlotUI();
            }
            else
            {
                Debug.Log("�������� ����������!");
            }
        }
    }

    private void UpdateSlotUI()
    {
        Image icon = GetComponent<Image>();
        if (icon != null)
        {
            icon.sprite = currentItem != null ? currentItem.itemIcon : null;
            icon.enabled = currentItem != null;
        }
    }
}
