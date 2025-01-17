using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField]
    private LevelLoader levelLoader;
    [SerializeField]
    private bool LoadNextScene = true;
    [SerializeField]
    private int SceneToLoad = 0;

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
        }
    }
}
