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
            GameMode = GameMode.Shared, // ��� Host/Client
            SessionName = "MySession",
            // Scene = null,  //  ������ �� ������� Scene
            SceneManager = sceneManager
        };

        var result = await runner.StartGame(startGameArgs);

        if (result.Ok)
        {
            Debug.Log(" ϳ��������� �� ��� ������!");
            PlayerSpawner spawner = FindObjectOfType<PlayerSpawner>();
            if (spawner != null)
            {
                spawner.SpawnMyPlayer(runner);
            }
            else
            {
                Debug.LogError(" PlayerSpawner �� �������� � ����!");
            }
        }
        else
        {
            Debug.LogError($" �� ������� �����������: {result.ShutdownReason}");
        }
    }
}
