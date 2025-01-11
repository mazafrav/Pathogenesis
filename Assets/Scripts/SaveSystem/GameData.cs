using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{
    public struct LevelData
    {
        public Vector2 playerPosition;
        public Transform[] mapElementsPosition;
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
        levels[levelName] = levelData;
    }

    public LevelData GetLevelData(string levelName) 
    {
        return levels[levelName];
    }
}
