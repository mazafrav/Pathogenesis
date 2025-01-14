using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDataLoader : MonoBehaviour
{
    private SaveSystem saveSystem;

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
        GameData.LevelData levelData = gameData.GetLevelData(SceneManager.GetActiveScene().name);

        if(levelData.playerPos.x != 0.0f && levelData.playerPos.y != 0.0f)
        {
            GameManager.Instance.GetPlayer().transform.position = levelData.playerPos.ConvertToUnityType();
        }
    }
}
