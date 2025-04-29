using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public string[] scenesToPlayMusic; // Назви сцен, де повинна грати музика
    private AudioSource audioSource;

    private void Awake()
    {
        // Запобігаємо дублюванню об'єкта
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null) return;

        // Перевіряємо, чи сцена має відтворювати музику
        bool shouldPlayMusic = false;
        foreach (string sceneName in scenesToPlayMusic)
        {
            if (scene.name == sceneName)
            {
                shouldPlayMusic = true;
                break;
            }
        }

        if (shouldPlayMusic)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
