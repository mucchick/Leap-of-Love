using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject saveGame1;
    private TextMeshProUGUI save1Text;
    [SerializeField] private GameObject saveGame2;
    private TextMeshProUGUI save2Text;
    [SerializeField] private GameObject saveGame3;
    private TextMeshProUGUI save3Text;
    [SerializeField] private GameObject backButton;


    private void Start()
    {
        save1Text = saveGame1.GetComponentInChildren<TextMeshProUGUI>();
        save2Text = saveGame2.GetComponentInChildren<TextMeshProUGUI>();
        save3Text = saveGame3.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(FetchSaveGames());
    }
    
    IEnumerator FetchSaveGames()
    {
        string userId = GameManager.Instance.UserId;
        string url = "http://localhost:8080/game/load/" + userId;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string responseData = webRequest.downloadHandler.text;
                Debug.Log("Received response: " + responseData);

                if (responseData.StartsWith("{") && responseData.EndsWith("}"))
                {
                    try
                    {
                        ProcessSaveGameData(responseData);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError("JSON Parsing Error: " + exception.Message);
                    }
                }
                else
                {
                    Debug.Log("No JSON data received. Response: " + responseData);
                }
            }
        }
    }

    public void OnSaveGameButtonClick(TextMeshProUGUI buttonText)
    {
        string sceneToLoad = "Level1";

        if (buttonText.text.Contains("Level 2"))
        {
            sceneToLoad = "Level2";
        }
        else if (buttonText.text.Contains("Level 3"))
        {
            sceneToLoad = "Level3";
        }

        SceneManager.LoadScene(sceneToLoad);
    }


    
    private void ProcessSaveGameData(string jsonData)
    {
        SaveGameResponse saveGameResponse = JsonUtility.FromJson<SaveGameResponse>(jsonData);

        UpdateSaveButton(saveGame1, save1Text, saveGameResponse.gameDataList.Length > 0 ? saveGameResponse.gameDataList[0] : null);
        UpdateSaveButton(saveGame2, save2Text, saveGameResponse.gameDataList.Length > 1 ? saveGameResponse.gameDataList[1] : null);
        UpdateSaveButton(saveGame3, save3Text, saveGameResponse.gameDataList.Length > 2 ? saveGameResponse.gameDataList[2] : null);
    }


    private void UpdateSaveButton(GameObject button, TextMeshProUGUI buttonText, SaveGameData saveData)
    {
        if (saveData != null)
        {
            // Assuming saveData.date is in the format "yyyy-MM-dd" and saveData.time is in the format "HH:mm"
            string dateTimeString = saveData.date + " " + saveData.time;
            DateTime dateTime;
        
            if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                buttonText.text = $"Level {saveData.currentLevel} - {dateTime:dd.MM.yyyy HH.mm}";
            }
            else
            {
                Debug.LogError("Invalid date format: " + dateTimeString);
                buttonText.text = "Invalid Date";
            }
        }
        else
        {
            buttonText.text = "Empty";
        }
    }

    public void playButtonClicked()
    {
        playButton.SetActive(false);
        saveGame1.SetActive(true);
        saveGame2.SetActive(true);
        saveGame3.SetActive(true);
        backButton.SetActive(true);
    }

    public void backButtonClicked()
    {
        playButton.SetActive(true);
        saveGame1.SetActive(false);
        saveGame2.SetActive(false);
        saveGame3.SetActive(false);
        backButton.SetActive(false);
    }
}