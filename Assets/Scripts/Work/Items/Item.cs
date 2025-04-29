using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string itemName;
    [SerializeField] public bool isQuestItem;
    public int costCopper;
    public int costSilver;
    public int costGold;
    public Sprite itemIcon;
    public bool isStackable;
    public int maxStackSize = 99;
    public int currentStackSize = 1;

    [System.NonSerialized] public GameObject prefab;        // НЕ серіалізувати префаб
    public ItemType itemType;
    [System.NonSerialized] public GameObject uiElement;     // НЕ серіалізувати UI-елемент

    public enum ItemType
    {
        None,
        Helmet,
        Chestplate,
        Gloves,
        Pants,
        Boots,
        Weapon,
        Shield,
        Accessory,
        Alchemical_Resources
    }
}
