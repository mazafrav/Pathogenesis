using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{
    bool isActive = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("SIIIIIIIUUUUUU");
            isActive = false;
            List<GameObject> gameObjectsToRespawn = GameManager.Instance.GetComponentInChildren<SaveGameObjectForRespawn>().gameObjectsToSave;


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
                }
            }

            // Saves the position stored in the RespawnCheckpoint prefab instance (this position is the prefab's first and only child) as the position to respawn
            GameManager.Instance.SetPlayerRespawnPosition(gameObject.transform.GetChild(0).position);
        }
    }
}
