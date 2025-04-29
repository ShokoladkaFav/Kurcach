using UnityEngine;

public class SkillSelectorVisible : MonoBehaviour
{
    [System.Serializable]
    public class Race
    {
        public string name; // Назва скіла
        public GameObject sprite; // Спрайт
    }

    public Race[] skills; // Масив для зберігання навичок
    private GameObject currentModels; // Поточна видима модель

    // Метод для вибору навички
    public void SelectSkillsNameVisible(int index)
    {
        // Ховаємо поточну модель, якщо вона існує
        if (currentModels != null)
        {
            currentModels.SetActive(false);
        }

        // Вмикаємо вибрану модель
        if (index >= 0 && index < skills.Length)
        {
            currentModels = skills[index].sprite;
            currentModels.SetActive(true);
        }
    }
}
