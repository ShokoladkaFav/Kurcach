using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UpdatePlayerData : MonoBehaviour
{
    private string updateUrl = "http://localhost/Kursah/update_player.php"; // ���� �� PHP-�������

    public void UpdatePlayer(int id, int level, int score)
    {
        StartCoroutine(SendUpdateRequest(id, level, score));
    }

    private IEnumerator SendUpdateRequest(int id, int level, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("level", level);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post(updateUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("��� ��������: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("������� ���������: " + www.error);
            }
        }
    }
}
