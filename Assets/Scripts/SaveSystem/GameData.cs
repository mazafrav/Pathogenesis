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
        lastLevel = "";
    }

    private Dictionary<string,LevelData> levels;
    private string lastLevel;

    public void SetLastLevelName(string name)
    {
        lastLevel = name;
    }

    public string GetCurrentLevelName()
    {
        return lastLevel;
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
}
