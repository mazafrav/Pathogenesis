using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField]
    public LevelLoader levelLoader;
    [SerializeField]
    public bool LoadNextScene = true;
    [SerializeField]
    public int SceneToLoad = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (LoadNextScene)
            {
                SceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
            }
            GameManager.Instance.GetPlayerController().UnregisterPlayerInputActions();
            levelLoader.StartLoadingLevel(SceneToLoad);

            GameManager.Instance.ClearRespawn();
        }
    }
}
