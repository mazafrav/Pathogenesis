using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDataLoader : MonoBehaviour
{
    private SaveSystem saveSystem;
    private GameData.LevelData levelData;
    private Enemy possessedEnemy = null;

    void Start()
    {
        saveSystem = GameManager.Instance.GetSaveSystem();

        //Save current level name
        if (SceneManager.GetActiveScene().buildIndex > 0) //Skip main menu
        {
            saveSystem.SaveCurrentLevelName(SceneManager.GetActiveScene().name);
        }

        //Load data from current level
        GameData gameData = saveSystem.GetGameData();
        levelData = gameData.GetLevelData(SceneManager.GetActiveScene().name);

        //Set which enemy is possessed
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (levelData.enemies != null && levelData.enemies.ContainsKey(enemies[i].name))
            {
                if (levelData.enemies[enemies[i].name].isPossessed)
                {
                    enemies[i].transform.position = levelData.playerPos.ConvertToUnityType();
                    possessedEnemy = enemies[i];

                    //Apply possession after x seconds to let the player initialize
                    Invoke(nameof(Possess), 0.01f);                 
                }
            }
        }

        //Set player position if it has a position stored and is not possessing an enemy
        if (levelData.playerPos.x != 0.0f && levelData.playerPos.y != 0.0f && possessedEnemy == null)
        {
            GameManager.Instance.GetPlayer().transform.position = levelData.playerPos.ConvertToUnityType();
        }

        //Set moving blocks data
        MovingBlockBase[] movingBlocks = FindObjectsOfType<MovingBlockBase>();
        for (int i = 0; i < movingBlocks.Length; i++)
        {
            string blockName = movingBlocks[i].transform.parent.name;

            if (levelData.movingBlocks != null && levelData.movingBlocks.ContainsKey(blockName))
            {
                movingBlocks[i].IsOpened = levelData.movingBlocks[blockName].isOpened;
                movingBlocks[i].NextPosition = levelData.movingBlocks[blockName].pos.ConvertToUnityType();
                movingBlocks[i].transform.position = movingBlocks[i].NextPosition;
            }
            else
            {
                movingBlocks[i].SetStartPosition();
            }
        }

        //Set checkpoints data
        RespawnCheckpoint[] respawnCheckpoints = FindObjectsOfType<RespawnCheckpoint>();
        for (int i = 0; i < respawnCheckpoints.Length;i++)
        {
            if(levelData.checkpoints != null && levelData.checkpoints.ContainsKey(respawnCheckpoints[i].name))
            {
                respawnCheckpoints[i].IsActive = levelData.checkpoints[respawnCheckpoints[i].name].isActive;
            }
        }

    }
  
    void Possess()
    {
        if(possessedEnemy)
        {
            HostAbsorption hostAbsorption = possessedEnemy.GetComponent<HostAbsorption>();
            hostAbsorption.SetPossessingEffectTime(0.7f);
            hostAbsorption.ApplyPossession(GameManager.Instance.GetPlayer());
        }
    }
}
