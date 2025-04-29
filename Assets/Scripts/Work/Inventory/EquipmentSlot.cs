using UnityEngine;
using UnityEngine.UI;
using static Item;

public class EquipmentSlot : MonoBehaviour
{
    public ItemType slotType; // Тип слота (шолом, нагрудник тощо)
    public Item currentItem; // Поточний предмет у слоті
    public Inventory inventory; // Посилання на інвентар

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
                currentItem = null; // Видаляємо предмет зі слота
                UpdateSlotUI();
            }
            else
            {
                Debug.Log("Інвентар заповнений!");
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
