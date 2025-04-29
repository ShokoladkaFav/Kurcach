using UnityEngine;

public class Shop : MonoBehaviour
{
    public Item[] shopItems; // ����� ��������, ��������� � �������

    private MoneyDisplay playerMoney;
    private Inventory inventory;

    void Start()
    {
        FindPlayer(); // ��������� ����� ��� ������ ������
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player"); // ��������� ������ �� ����

        if (playerObj != null)
        {
            playerMoney = playerObj.GetComponent<MoneyDisplay>(); // �������� ������ ������

            // ������ Inventory ����� ������� ��'���� ������
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
            Debug.LogWarning("��������� ����� ������...");
            FindPlayer(); // ��������� ����� ������, ���� ���� �� ����
        }

        if (playerMoney == null)
        {
            Debug.LogError("MoneyDisplay �� �������� � ������!");
            return;
        }

        if (inventory == null)
        {
            Debug.LogError("Inventory �� �������� � ������!");
            return;
        }

        if (itemIndex < 0 || itemIndex >= shopItems.Length)
        {
            Debug.LogError("����������� ������ ������!");
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
                Debug.Log($"������� ����� {itemToBuy.itemName}!");
            }
            else
            {
                Debug.LogError("������� ��� ����� ������!");
            }
        }
        else
        {
            Debug.Log("�� ������� ������!");
        }
    }
}