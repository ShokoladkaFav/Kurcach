using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Canvas Pause Menu
    public GameObject inventoryUI; // Canvas Inventory
    public GameObject achievementsUI;  // Canvas Achievements
    public GameObject tasksUI;         // Canvas Tasks
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // якщо в≥дкрите в≥кно завдань, закриваЇмо його
            if (tasksUI != null && tasksUI.activeSelf)
            {
                CloseTasks();
            }
            // якщо в≥дкрите в≥кно дос€гнень, закриваЇмо його
            else if (achievementsUI != null && achievementsUI.activeSelf)
            {
                CloseAchievements();
            }
            // якщо в≥дкрите меню паузи, в≥дновлюЇмо гру
            else if (isPaused)
            {
                ResumeGame();
            }
            // якщо н≥чого не в≥дкрито, ставимо гру на паузу
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    void PauseGame()
    {
        // «акрити ≥нвентар перед в≥дкритт€м меню паузи
        if (inventoryUI != null && inventoryUI.activeSelf)
        {
            inventoryUI.SetActive(false);
        }

        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void OpenTasks()
    {
        tasksUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Debug.Log("¬≥дкритт€ завдань");
    }

    public void CloseTasks()
    {
        tasksUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void OpenAchievements()
    {
        achievementsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Debug.Log("¬≥дкритт€ дос€гнень");
    }

    public void CloseAchievements()
    {
        achievementsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
