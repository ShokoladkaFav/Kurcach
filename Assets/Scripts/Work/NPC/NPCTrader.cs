using UnityEngine;

public class NPC_Trader : MonoBehaviour
{
    public GameObject dialogueUI; // ������ ��� ������
    public GameObject shopUI;     // ������ ��������

    private bool isPlayerNearby = false;
    private bool isShopOpen = false; // ����� ��� �������������� ����� ��������

    void Update()
    {
        // ���� ������� ������� � ������� E, ����������� ����� (���� ������� �� ��������)
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isShopOpen)
        {
            ToggleDialogue();
        }
    }

    private void ToggleDialogue()
    {
        // ����������� ����� ���������� ����
        bool isActive = dialogueUI.activeSelf;
        dialogueUI.SetActive(!isActive);

        if (isActive)
        {
            Debug.Log("ĳ���� �������");
        }
        else
        {
            Debug.Log("ĳ���� �������");
        }
    }

    public void OpenShop()
    {
        // ������� �����, ���� �� ��������
        dialogueUI.SetActive(false);
        Debug.Log("ĳ���� ��������");

        // ³������ �������
        shopUI.SetActive(true);
        isShopOpen = true; // ���������, �� ������� ��������
        Debug.Log("������� ��������");
    }

    public void CloseShop()
    {
        // ������� �������
        shopUI.SetActive(false);
        isShopOpen = false; // ���������, �� ������� ��������
        Debug.Log("������� ��������");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("������� � ��� ��������");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // ������� �� ���� �� ������� ���� ��������
            dialogueUI.SetActive(false);
            shopUI.SetActive(false);
            isShopOpen = false; // ������� ���� ��������
            Debug.Log("������� ������ �� ���� ��������");
        }
    }
}
