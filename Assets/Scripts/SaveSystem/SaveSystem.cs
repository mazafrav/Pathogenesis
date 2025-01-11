using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem 
{
    public SaveSystem()
    {

    }

    public void SaveCurrentLevelName(string lastLevelName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GameData.virus";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData();
        data.SetLastLevelName(lastLevelName);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public GameData LoadCurrentLevelName()
    {
        string path = Application.persistentDataPath + "/GameData.virus";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
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
