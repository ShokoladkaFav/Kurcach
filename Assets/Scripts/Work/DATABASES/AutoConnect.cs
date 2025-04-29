using UnityEngine;
using Fusion;
using System.Threading.Tasks;

public class AutoConnect : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab;
    private NetworkRunner runner;

    private async void Start()
    {
        runner = Instantiate(networkRunnerPrefab);
        runner.name = "NetworkRunner";
        runner.ProvideInput = true;

        var sceneManager = runner.GetComponent<NetworkSceneManagerDefault>();
        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        var startGameArgs = new StartGameArgs
        {
            GameMode = GameMode.Shared, // або Host/Client
            SessionName = "MySession",
            // Scene = null,  //  просто не вказуємо Scene
            SceneManager = sceneManager
        };

        var result = await runner.StartGame(startGameArgs);

        if (result.Ok)
        {
            Debug.Log(" Підключення до сесії успішне!");
            PlayerSpawner spawner = FindObjectOfType<PlayerSpawner>();
            if (spawner != null)
            {
                spawner.SpawnMyPlayer(runner);
            }
            else
            {
                Debug.LogError(" PlayerSpawner не знайдено у сцені!");
            }
        }
        else
        {
            Debug.LogError($" Не вдалося підключитися: {result.ShutdownReason}");
        }
    }
}
