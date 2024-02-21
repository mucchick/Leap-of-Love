using System;

[System.Serializable]
public class SaveGameData
{
    public string date;
    public string time;
    public int currentLevel;
}

[System.Serializable]
public class SaveGameList
{
    public SaveGameData[] saveGames;
}

[System.Serializable]
public class SaveGameResponse
{
    public string userId;
    public SaveGameData[] gameDataList;
}

