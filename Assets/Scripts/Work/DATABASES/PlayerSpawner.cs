using UnityEngine;
using Fusion;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] racePrefabs; // Префаби для рас

    public void SpawnMyPlayer(NetworkRunner runner)
    {
        StartCoroutine(SpawnCoroutine(runner));
    }

    private IEnumerator SpawnCoroutine(NetworkRunner runner)
    {
        string username = PlayerPrefs.GetString("Username", "");
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError(" Username не знайдено!");
            yield break;
        }

        string url = "http://localhost/Kursach/get_race.php?username=" + username;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($" Помилка при запиті раси: {webRequest.error}");
                yield break;
            }

            string json = webRequest.downloadHandler.text;
            RaceData raceData = JsonUtility.FromJson<RaceData>(json);

            if (!string.IsNullOrEmpty(raceData.error))
            {
                Debug.LogError($" Помилка у відповіді: {raceData.error}");
                yield break;
            }

            int raceIndex = raceData.race_id - 1;
            if (raceIndex < 0 || raceIndex >= racePrefabs.Length)
            {
                Debug.LogError($" Некоректний індекс раси: {raceData.race_id}");
                yield break;
            }

            Vector3 spawnPosition = GetSpawnPosition();
            runner.Spawn(racePrefabs[raceIndex], spawnPosition, Quaternion.identity, runner.LocalPlayer);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }

    [System.Serializable]
    private class RaceData
    {
        public int race_id;
        public string error;
    }
}
