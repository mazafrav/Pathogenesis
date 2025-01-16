using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameData
{
    #region Data Structs

    [Serializable]
    public struct EnemyData
    {
        public bool isPossessed;
    }

    [Serializable]
    public struct MovingBlockData
    {
        public Pos pos;
        public bool isOpened;
    }

    [Serializable]
    public struct Pos
    {
        public float x;
        public float y;

        public readonly Vector3 ConvertToUnityType()
        {
            return new Vector3(x, y, 0.0f);
        }

        public void ConvertToPosType(float newX, float newY)
        {
            x = newX;
            y = newY;
        }
    }

    [Serializable]
    public class LevelData
    {
        public Pos playerPos;
        public Dictionary<string, MovingBlockData> movingBlocks;
        public Dictionary<string, EnemyData> enemies;
        public string possessedEnemy;

        public LevelData()
        {
            movingBlocks = new Dictionary<string, MovingBlockData>();
            enemies = new Dictionary<string, EnemyData>();
            possessedEnemy = null;
            playerPos.x = 0.0f;
            playerPos.y = 0.0f;
        }

        public void AddMovingBlockData(string blockName, MovingBlockData blockData)
        {
            if(!movingBlocks.ContainsKey(blockName))
            {
                movingBlocks.Add(blockName, blockData);
            }
        }

        public void AddEnemyData(string enemyName, EnemyData enemyData)
        {
            if (!enemies.ContainsKey(enemyName))
            {
                enemies.Add(enemyName, enemyData);
            }
        }
    }

    #endregion

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
