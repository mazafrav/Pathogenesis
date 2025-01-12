using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem 
{
    private GameData gameData = null;
    string path = Application.persistentDataPath + "/GameData.virus";

    public SaveSystem()
    {
        Init();
    }

    public GameData GetGameData() { return gameData; }

    public void SaveCurrentLevelName(string lastLevelName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, File.Exists(path)? FileMode.Open: FileMode.Create);
      
        gameData.SetLastLevelName(lastLevelName);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public void SaveCurrentLevelState(string currentLevelName, GameData.LevelData levelData)
    {
        BinaryFormatter formatter = new BinaryFormatter();      
        FileStream stream = new FileStream(path, File.Exists(path) ? FileMode.Open : FileMode.Create);
        
        gameData.SetLevelData(currentLevelName, levelData);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public void DeleteGameData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private void Init()
    {
        if(!File.Exists(path))
        {
            gameData = new GameData();
        }
        else
        {
            gameData = LoadGameData();
        }
    }

    private GameData LoadGameData()
    {      
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, File.Exists(path) ? FileMode.Open : FileMode.Create);
            GameData gameData = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return gameData;
        }
        else
        {
            return null;
        }
    }
}
