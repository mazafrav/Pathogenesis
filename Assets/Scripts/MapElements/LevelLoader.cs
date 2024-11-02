using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    [SerializeField]
    public float transitionTime = 0.7f;
   
    //private float timeToResetLevel = 1.0f;
    //private float currentTimeToResetLevel = 0.0f;

    private void Start()
    {
        //currentTimeToResetLevel = timeToResetLevel;

        Dictionary<int, Vector3> respawnValues = GameManager.Instance.GetRespawnValues();
        if (respawnValues.Count > 0)
        {
            List<GameObject> gameObjectsToRespawn = GameObject.Find("RespawnLoader").GetComponent<SaveGameObjectForRespawn>().gameObjectsToSave;

            for (int i = 0; i < gameObjectsToRespawn.Count; i++)
            {
                if (gameObjectsToRespawn[i] != null && !respawnValues.ContainsKey(i))
                {
                    Destroy(gameObjectsToRespawn[i]);
                }
            }

            foreach (KeyValuePair<int, Vector3> entry in respawnValues)
            {
                gameObjectsToRespawn[entry.Key].transform.position = entry.Value;
            }

            //GameManager.Instance.GetPlayerController().gameObject.transform.position = GameManager.Instance.GetPlayerRespawnPosition();

        }

        if (GameManager.Instance.GetPlayerRespawnPosition() != Vector3.zero)
        {
            GameManager.Instance.GetPlayerController().gameObject.transform.position = GameManager.Instance.GetPlayerRespawnPosition();
        }
    }

    private void Update()
    {
        //We reset the level when pressing a key for X seconds
        //if(Input.GetKey(KeyCode.R) && currentTimeToResetLevel > 0.0f)
        //{
        //    currentTimeToResetLevel -= Time.deltaTime;
        //}

        //if(currentTimeToResetLevel <= 0.0f)
        //{
        //    RestartLevel();
        //    currentTimeToResetLevel = timeToResetLevel;
        //}
    }

    public void StartLoadingLevel(int level)
    {
        StartCoroutine(LoadLevel(level));
    }

    public void RestartLevel()
    {
        //GameManager.Instance.SetMusicSelectionIndex(1);
        GameManager.Instance.GetPlayerController().UnregisterPlayerInputActions();
        GameManager.Instance.GetPlayerController().GetPlayerIAs().Disable();
        StartLoadingLevel(SceneManager.GetActiveScene().buildIndex);


        GameManager.Instance.ClearRespawn();
    }

    public void CheckRespawn()
    {
        if (GameManager.Instance.GetRespawnValues().Count > 0 || GameManager.Instance.GetPlayerRespawnPosition() != Vector3.zero)
        {
            //GameManager.Instance.SetMusicSelectionIndex(1);
            GameManager.Instance.GetPlayerController().UnregisterPlayerInputActions();
            StartLoadingLevel(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            RestartLevel();
        }

    }

    IEnumerator LoadLevel(int level)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(level);
        GameManager.Instance.SetLastLevelPlayed(level);
    }
}
