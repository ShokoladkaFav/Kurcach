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
            // ���� ������� ���� �������, ��������� ����
            if (tasksUI != null && tasksUI.activeSelf)
            {
                CloseTasks();
            }
            // ���� ������� ���� ���������, ��������� ����
            else if (achievementsUI != null && achievementsUI.activeSelf)
            {
                CloseAchievements();
            }
            // ���� ������� ���� �����, ���������� ���
            else if (isPaused)
            {
                ResumeGame();
            }
            // ���� ����� �� �������, ������� ��� �� �����
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
        // ������� �������� ����� ��������� ���� �����
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
        Debug.Log("³������� �������");
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
        Debug.Log("³������� ���������");
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
