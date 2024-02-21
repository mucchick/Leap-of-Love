using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string UserId { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeUserId();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeUserId()
    {
        UserId = PlayerPrefs.GetString("UserId", "");
        Debug.Log("Retrieved UserId: " + UserId);
        if (string.IsNullOrEmpty(UserId))
        {
            UserId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("UserId", UserId);
            PlayerPrefs.Save();
            Debug.Log("Generated new UserId: " + UserId);
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "Main Menu") 
        {
            Debug.Log($"Scene Loaded: {scene.name}");
            StartCoroutine(SendLevelData(scene.name));
        }
        else
        {
            Debug.Log("MainMenu scene loaded, not sending data.");
        }
    }

    IEnumerator SendLevelData(string levelName)
    {
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm");
        int currentLevel = 0;

        if (levelName.StartsWith("Level") && int.TryParse(levelName.Replace("Level", ""), out currentLevel))
        {
            SaveGameData levelData = new SaveGameData()
            {
                date = dateTime,
                time = time,
                currentLevel = currentLevel
            };

            string json = JsonUtility.ToJson(levelData);
            Debug.Log("Sending level data: " + json);

            string url = "http://localhost:8080/game/save?userId=" + UserId;
            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log("Level data sent successfully. Response: " + webRequest.downloadHandler.text);
                }
            }
        }
        else
        {
            Debug.LogError("Invalid level name format: " + levelName);
        }
    }

}