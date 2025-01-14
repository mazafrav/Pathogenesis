using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnCheckpoint : MonoBehaviour
{
    [SerializeField]
    GameObject checkpointAnimUI;

    private bool isActive = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")))
        {        
            isActive = false;

            GameData.LevelData levelData = new GameData.LevelData();
            levelData.playerXposition = collision.transform.position.x;
            levelData.playerYposition = collision.transform.position.y;

            SaveSystem saveSystem = GameManager.Instance.GetSaveSystem();
            saveSystem.SaveCurrentLevelState(SceneManager.GetActiveScene().name, levelData);
            saveSystem?.onSave.Invoke();

            Instantiate(checkpointAnimUI);
        }
    }
}
