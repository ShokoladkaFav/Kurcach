using UnityEngine;

public class NPCAnimationSwitch : MonoBehaviour
{
    private Animator animator;
    private bool isSpecial = false;

    public float idleTime = 5f; // ���, �������� ����� ���� ������������� Idle
    private float timer;
    private bool specialPlayed = false; // ��� ������������, �� Special ���������� ����� ���� ���

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = idleTime;
    }

    void Update()
    {
        // ���� ����� ������������ Special, �� ������, ���� ���� ����������
        if (isSpecial && !animator.GetCurrentAnimatorStateInfo(0).IsName("Special"))
        {
            isSpecial = false; // ����������� �� Idle ���� ���������� Special
            animator.SetBool("IsSpecial", false);
            timer = idleTime; // �������� ����� ��� ��� Idle
            specialPlayed = false; // ������� ������, ��� Special ����� ���� ����� ��������
        }

        // ���� ����� ������������ Idle, �� ������ �� ����
        if (!isSpecial)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && !specialPlayed)
            {
                // ���������� �� Special
                isSpecial = true;
                animator.SetBool("IsSpecial", true);
                specialPlayed = true; // ��������, �� Special ��� ������������
            }
        }
    }
}
