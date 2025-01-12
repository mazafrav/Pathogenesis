using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameData 
{
    [Serializable]
    public struct LevelData
    {
        public float playerXposition;
        public float playerYposition;
        //public Transform[] mapElementsPosition;
    }


    public GameData()
    {
        levels = new Dictionary<string, LevelData>();      
        currentLevel = "";
    }

    private Dictionary<string,LevelData> levels;
    private string currentLevel;

    public void SetCurrentLevelName(string name)
    {
        currentLevel = name;
    }

    public string GetCurrentLevelName()
    {
        return currentLevel;
    }

    public void SetLevelData(string levelName, LevelData levelData)
    {
        if(levels.ContainsKey(levelName))
        {
            levels[levelName] = levelData;
        }
        else
        {
            levels.Add(levelName, levelData);
        }
    }

    public LevelData GetLevelData(string levelName) 
    {
        if (levels.ContainsKey(levelName))
        {      
             return levels[levelName];
        }
        else
        {
            return new LevelData();
        }
    }

    public void DeleteLevelData(string levelName) 
    {
        if (levels.ContainsKey(levelName)) 
        {
            levels.Remove(levelName);
        }
    }
}
