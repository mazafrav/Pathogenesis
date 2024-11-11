using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{
    [SerializeField]
    GameObject checkpointAnimUI;

    bool isActive = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && (collision.gameObject.CompareTag("Player") || 
            (collision.gameObject.CompareTag("Enemy") && collision.GetComponentInParent<PlayerController>())))
        {
            //Debug.Log("SIIIIIIIUUUUUU");
            isActive = false;
            SaveGameObjectForRespawn respawnLoader = GameObject.FindObjectOfType<SaveGameObjectForRespawn>();

            if (respawnLoader != null)
            {

                List<GameObject> gameObjectsToRespawn = respawnLoader.gameObjectsToSave;


                // Reset respawn values when you reach a nearer checkpoint
                if (GameManager.Instance.GetRespawnValues().Count > 0)
                {
                    GameManager.Instance.ResetRespawnValues();
                }

                for (int i = 0; i < gameObjectsToRespawn.Count; i++)
                {
                    if (gameObjectsToRespawn[i] != null)
                    {
                        GameManager.Instance.AddRespawnValue(i, gameObjectsToRespawn[i].transform.position);

                        if (collision.gameObject.CompareTag("Enemy") && gameObjectsToRespawn[i] == collision.gameObject)
                        {
                            //Debug.Log("oh yesyeyses");
                            GameManager.Instance.SetPossessedEnemyToRespawn(i);
                        }
                    }
                    else
                    {
                        GameManager.Instance.AddRespawnValue(i, Vector3.zero);
                    }
                }
            }

            // Saves the position stored in the RespawnCheckpoint prefab instance (this position is the prefab's first and only child) as the position to respawn
            GameManager.Instance.SetPlayerRespawnPosition(gameObject.transform.GetChild(0).position);
            Instantiate(checkpointAnimUI);
        }
    }
}
