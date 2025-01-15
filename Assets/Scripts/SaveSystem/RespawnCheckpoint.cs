using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnCheckpoint : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    GameObject checkpointAnimUI;

    private bool isActive = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (isActive && (collision.gameObject.CompareTag("Player") || (enemy && enemy.IsPossesed)))
        {
            isActive = false;

            GameData.LevelData levelData = new GameData.LevelData();

            levelData.playerPos.ConvertToPosType(spawnPoint.position.x, spawnPoint.position.y);
            levelData.possessedEnemy = enemy ? enemy.GetType().ToString() : null;
            SaveSystem saveSystem = GameManager.Instance.GetSaveSystem();
            saveSystem.SaveCurrentLevelState(SceneManager.GetActiveScene().name, levelData);
            saveSystem?.onSave.Invoke();

            Instantiate(checkpointAnimUI);
        }
    }
}
