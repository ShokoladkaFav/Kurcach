using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Метод для завантаження сцени за назвою
    public void LoadScene(string RegisterMenu)
    {
        SceneManager.LoadScene(RegisterMenu);
    }
}
