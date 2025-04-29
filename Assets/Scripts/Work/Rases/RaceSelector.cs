using UnityEngine;

public class RaceSelector : MonoBehaviour
{
    [System.Serializable]
    public class Race
    {
        public string name; // Назва раси
        public GameObject model; // Модель або спрайт персонажа
    }

    public Race[] races; // Масив для зберігання рас
    private GameObject currentModel; // Поточна видима модель

    // Метод для вибору раси
    public void SelectRace(int index)
    {
        // Ховаємо поточну модель, якщо вона існує
        if (currentModel != null)
        {
            currentModel.SetActive(false);
        }

        // Вмикаємо вибрану модель
        if (index >= 0 && index < races.Length)
        {
            currentModel = races[index].model;
            currentModel.SetActive(true);

            // Відтворюємо анімацію, якщо є
            Animator animator = currentModel.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Idle"); // Назва анімації (замість "Idle" можна вказати вашу)
            }
        }
    }
}
