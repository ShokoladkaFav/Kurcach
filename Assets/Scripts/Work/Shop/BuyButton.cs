using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public Shop shop;  // Посилання на магазин
    public int itemIndex;  // Індекс предмета в магазині

    // Метод, який викликається при натисканні на кнопку
    public void OnBuyButtonClicked()
    {
        Debug.Log("Кнопка покупки була нажата");
        // Викликаємо метод покупки предмета за індексом
        shop.TryBuy(itemIndex);
        Debug.Log("Покупка предмета за індексом спрацювала");
    }
}
