using UnityEngine;
using Fusion;
using System.Collections;

public class InventoryToggle : NetworkBehaviour
{
    private GameObject inventoryUI;
    private bool isInventoryOpen = false;

    void Start()
    {
        if (!HasInputAuthority) return;

        StartCoroutine(WaitForInventory());
    }

    private IEnumerator WaitForInventory()
    {
        while (inventoryUI == null)
        {
            FindInventory();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void FindInventory()
    {
        if (inventoryUI != null) return;

        Transform inventoryTransform = transform.Find("INVENTORY");

        if (inventoryTransform == null)
        {
            inventoryTransform = transform.GetComponentInChildren<Transform>(true);
        }

        if (inventoryTransform != null)
        {
            inventoryUI = inventoryTransform.gameObject;
            Debug.Log("Inventory UI знайдено: " + inventoryUI.name + " у " + gameObject.name);
        }
        else
        {
            Debug.LogWarning("INVENTORY не знайдено у " + gameObject.name + ". Чекаємо...");
        }
    }

    void Update()
    {
        if (!HasInputAuthority) return;
        if (inventoryUI == null) return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (isInventoryOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogError("inventoryUI все ще null! Переконайтеся, що об'єкт INVENTORY є у гравця.");
            return;
        }

        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
    }
}
