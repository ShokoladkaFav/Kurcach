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
                    Debug.Log($"[InventoryItemUI] ����������� ����'����� item {linkedItem.itemName} (ID: {linkedItem.id})");
                    break;
                }
            }
        }

        if (linkedItem == null)
        {
            Debug.LogError("[InventoryItemUI] Start: �� ������� ����'����� Item!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (linkedItem == null)
            {
                Debug.LogWarning("[InventoryItemUI] linkedItem == null, �������� ���.");
                return;
            }
            if (linkedItem.id <= 0)
            {
                Debug.LogWarning($"[InventoryItemUI] ����������� ID � �������� ({linkedItem.itemName}), �������� ���.");
                return;
            }

            Debug.Log($"[InventoryItemUI] ��� �� ��������: {linkedItem.itemName} (ID: {linkedItem.id})");

            Inventory.instance.HandleItemRightClick(linkedItem);
        }
    }
}
