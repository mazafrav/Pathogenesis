using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDataLoader : MonoBehaviour
{
    private SaveSystem saveSystem;
    private GameData.LevelData levelData;
    private Enemy possessedEnemy;

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

        //Set player position if it has a position stored and is not possessing an enemy
        if(levelData.playerPos.x != 0.0f && levelData.playerPos.y != 0.0f && levelData.possessedEnemy == null)
        {
            GameManager.Instance.GetPlayer().transform.position = levelData.playerPos.ConvertToUnityType();
        }

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
    }
  
    void Possess()
    {
        possessedEnemy.GetComponent<HostAbsorption>().ApplyPossession(GameManager.Instance.GetPlayer());
    }
}
