using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public Shop shop;  // ��������� �� �������
    public int itemIndex;  // ������ �������� � �������

    // �����, ���� ����������� ��� ��������� �� ������
    public void OnBuyButtonClicked()
    {
        Debug.Log("������ ������� ���� ������");
        // ��������� ����� ������� �������� �� ��������
        shop.TryBuy(itemIndex);
        Debug.Log("������� �������� �� �������� ����������");
    }
}
