using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnCheckpoint : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    GameObject checkpointAnimUI;

    public bool IsActive { get; set; } = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (IsActive && (collision.gameObject.CompareTag("Player") || (enemy && enemy.IsPossesed)))
        {
            Instantiate(checkpointAnimUI);
            IsActive = false;

            GameData.LevelData levelData = new GameData.LevelData();

            //Save player data
            levelData.playerPos.ConvertToPosType(spawnPoint.position.x, spawnPoint.position.y);
            //levelData.possessedEnemy = enemy ? enemy.GetType().ToString() : null;

            //Save enemies data
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy e in enemies)
            {
                GameData.EnemyData enemyData = new GameData.EnemyData();
                enemyData.isPossessed = e.IsPossesed;
                levelData.AddEnemyData(e.name,enemyData);
            }

            //Save moving blocks data
            MovingBlockBase[] movingBlocks = FindObjectsOfType<MovingBlockBase>();
            foreach (MovingBlockBase block in movingBlocks)
            {
                GameData.MovingBlockData movingBlockData = new GameData.MovingBlockData();
                movingBlockData.pos.ConvertToPosType(block.transform.position.x, block.transform.position.y);
                movingBlockData.isOpened = block.IsOpened;
                levelData.AddMovingBlockData(block.transform.parent.name, movingBlockData);
            }

            //Save checkpoints data
            GameData.CheckpointData checkpointData = new GameData.CheckpointData();
            checkpointData.isActive = IsActive;
            levelData.AdCheckpointData(transform.name, checkpointData);



            //Save all data. This must be at the end
            SaveSystem saveSystem = GameManager.Instance.GetSaveSystem();
            saveSystem.SaveCurrentLevelState(SceneManager.GetActiveScene().name, levelData);
        }
    }
}
