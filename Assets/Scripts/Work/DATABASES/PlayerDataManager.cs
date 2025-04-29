using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using Fusion;

public class PlayerDataLoader : NetworkBehaviour
{
    [Networked] public NetworkString<_16> PlayerUsername { get; set; }
    [Networked] public int PlayerLevel { get; set; }

    public Text playerNameText;

    private void Start()
    {
        if (Object.HasInputAuthority) // ¬иконуЇтьс€ лише дл€ локального гравц€
        {
            int playerID = PlayerPrefs.GetInt("UserID", -1);
            if (playerID == -1)
            {
                Debug.LogError("Player ID is missing!");
                return;
            }

            StartCoroutine(GetPlayerData(playerID));
        }
    }

    IEnumerator GetPlayerData(int playerID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", playerID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Kursach/get_player_data.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                PlayerInfo playerInfo = JsonUtility.FromJson<PlayerInfo>(json);

                if (!string.IsNullOrEmpty(playerInfo.username))
                {
                    PlayerUsername = playerInfo.username;
                    PlayerLevel = playerInfo.level;
                    UpdatePlayerUI();
                }
                else
                {
                    Debug.LogError("Player not found!");
                }
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }

    private void UpdatePlayerUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = $"{PlayerUsername} (Lv. {PlayerLevel})";
        }
    }

    [System.Serializable]
    private class PlayerInfo
    {
        public string username;
        public int level;
    }
}
