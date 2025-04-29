using UnityEngine;

public class NPCAnimationSwitch : MonoBehaviour
{
    private Animator animator;
    private bool isSpecial = false;

    public float idleTime = 5f; // Час, протягом якого буде відтворюватися Idle
    private float timer;
    private bool specialPlayed = false; // Щоб переконатися, що Special програється тільки один раз

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = idleTime;
    }

    void Update()
    {
        // Якщо зараз відтворюється Special, то чекаємо, поки вона закінчиться
        if (isSpecial && !animator.GetCurrentAnimatorStateInfo(0).IsName("Special"))
        {
            isSpecial = false; // Повертаємося до Idle після завершення Special
            animator.SetBool("IsSpecial", false);
            timer = idleTime; // Починаємо знову час для Idle
            specialPlayed = false; // Скидаємо прапор, щоб Special могла бути знову програна
        }

        // Якщо зараз відтворюється Idle, то чекаємо на зміну
        if (!isSpecial)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && !specialPlayed)
            {
                // Перемикаємо на Special
                isSpecial = true;
                animator.SetBool("IsSpecial", true);
                specialPlayed = true; // Записуємо, що Special вже програвалася
            }
        }
    }
}
