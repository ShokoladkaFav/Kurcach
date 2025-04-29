using UnityEngine;

public class RaceSelector : MonoBehaviour
{
    [System.Serializable]
    public class Race
    {
        public string name; // ����� ����
        public GameObject model; // ������ ��� ������ ���������
    }

    public Race[] races; // ����� ��� ��������� ���
    private GameObject currentModel; // ������� ������ ������

    // ����� ��� ������ ����
    public void SelectRace(int index)
    {
        // ������ ������� ������, ���� ���� ����
        if (currentModel != null)
        {
            currentModel.SetActive(false);
        }

        // ������� ������� ������
        if (index >= 0 && index < races.Length)
        {
            currentModel = races[index].model;
            currentModel.SetActive(true);

            // ³��������� �������, ���� �
            Animator animator = currentModel.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Idle"); // ����� ������� (������ "Idle" ����� ������� ����)
            }
        }
    }
}
