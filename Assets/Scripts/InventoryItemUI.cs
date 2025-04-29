using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerClickHandler
{
    public Item linkedItem;

    private void Start()
    {
        if (linkedItem == null && Inventory.instance != null)
        {
            foreach (var item in Inventory.instance.items)
            {
                if (item.uiElement == gameObject)
                {
                    linkedItem = item;
                    Debug.Log($"[InventoryItemUI] Автоматично прив'язано item {linkedItem.itemName} (ID: {linkedItem.id})");
                    break;
                }
            }
        }

        if (linkedItem == null)
        {
            Debug.LogError("[InventoryItemUI] Start: Не вдалося прив'язати Item!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (linkedItem == null)
            {
                Debug.LogWarning("[InventoryItemUI] linkedItem == null, ігноруємо клік.");
                return;
            }
            if (linkedItem.id <= 0)
            {
                Debug.LogWarning($"[InventoryItemUI] Некоректний ID у предмета ({linkedItem.itemName}), ігноруємо клік.");
                return;
            }

            Debug.Log($"[InventoryItemUI] ПКМ по предмету: {linkedItem.itemName} (ID: {linkedItem.id})");

            Inventory.instance.HandleItemRightClick(linkedItem);
        }
    }
}
