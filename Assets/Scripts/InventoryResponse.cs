using System;
using System.Collections.Generic;

[Serializable]
public class InventoryResponse
{
    public bool success;
    public string message;
    public List<InventoryItemData> items;
}

[Serializable]
public class InventoryItemData
{
    public int id;
    public string itemName;
    public int itemAmount;
    public string iconPath;
}
