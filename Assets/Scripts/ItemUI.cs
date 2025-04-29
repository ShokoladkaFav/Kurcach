using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI goldPriceText;
    public TextMeshProUGUI silverPriceText;
    public TextMeshProUGUI copperPriceText;
    public Image itemIcon;
    public Button buyButton;

    private Item currentItem;

    public void Setup(Item item)
    {
        currentItem = item;

        // Додано для діагностики
        Debug.Log($"[ItemUI] Setup: itemName = {item.itemName}, ID = {item.id}");

        itemNameText.text = item.itemName;
        goldPriceText.text = item.costGold.ToString();
        silverPriceText.text = item.costSilver.ToString();
        copperPriceText.text = item.costCopper.ToString();

        // Іконка
        itemIcon.sprite = item.itemIcon;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            // Додано для діагностики
            Debug.Log($"[ItemUI] BuyButton clicked: item ID = {currentItem.id}, itemName = {currentItem.itemName}");
            ShopManager.Instance.BuyItem(currentItem);
        });
    }
}
