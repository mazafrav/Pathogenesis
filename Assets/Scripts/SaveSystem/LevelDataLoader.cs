using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cinemachine;


public class LevelDataLoader : MonoBehaviour
{
    private SaveSystem saveSystem;
    private GameData.LevelData levelData;
    GameObject possessedEnemy;
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

        // Load the enemy prefab using Addressables
        if(levelData.possessedEnemy != null)
        {
            Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Enemies/" + levelData.possessedEnemy + ".prefab").Completed += OnPrefabLoaded;
        }
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Instantiate the prefab in the scene
            possessedEnemy = Instantiate(handle.Result, levelData.playerPos.ConvertToUnityType(), Quaternion.identity);            
            Invoke(nameof(Possess), 0.001f);
        }
        else
        {
            Debug.LogError("Failed to load prefab via Addressables.");
        }
    }

    void Possess()
    {
        possessedEnemy.GetComponent<HostAbsorption>().ApplyPossession(GameManager.Instance.GetPlayer());

    }
}
