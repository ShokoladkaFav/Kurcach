using UnityEngine;

public class DialogSwitcher : MonoBehaviour
{
    public GameObject currentDialogPanel;  // �������� ����� (������)
    public GameObject nextDialogPanel;     // ��������� ����� (������)

    private bool isPlayerNearby = false;

    // ����� ��� ����������� �� �������� ������
    public void SwitchToNextDialog()
    {
        currentDialogPanel.SetActive(false);  // ��������� �������� �����
        nextDialogPanel.SetActive(true);      // �������� ��������� �����
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // ������� �� ���� �� ������� ���� ��������
            currentDialogPanel.SetActive(false);
            nextDialogPanel.SetActive(false);
        }
    }
}
