using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    [SerializeField]
    public float transitionTime = 0.7f;

    public delegate void OnLevelRestart();
    public OnLevelRestart onLevelRestart;
   
    //private float timeToResetLevel = 1.0f;
    //private float currentTimeToResetLevel = 0.0f;

    private void Start()
    {
        //currentTimeToResetLevel = timeToResetLevel;

    

       // StartCoroutine(LoadCheckpointValues());
    }

    //IEnumerator LoadCheckpointValues()
    //{
    //    yield return null;

    //    Dictionary<int, Vector3> respawnValues = GameManager.Instance.GetRespawnValues();
    //    if (respawnValues.Count > 0)
    //    {
    //        List<GameObject> gameObjectsToRespawn = GameObject.FindObjectOfType<SaveGameObjectForRespawn>().gameObjectsToSave;

    //        for (int i = 0; i < gameObjectsToRespawn.Count; i++)
    //        {
    //            if (gameObjectsToRespawn[i] != null && (respawnValues.ContainsKey(i) && respawnValues[i].Equals(Vector3.zero)))
    //            {
    //                Destroy(gameObjectsToRespawn[i]);
    //            }
    //        }

    //        foreach (KeyValuePair<int, Vector3> entry in respawnValues)
    //        {
    //            if (gameObjectsToRespawn[entry.Key] != null)
    //            {
    //                gameObjectsToRespawn[entry.Key].transform.position = entry.Value;
    //            }
    //        }

    //        GameManager.Instance.GetPlayerController().gameObject.transform.position = GameManager.Instance.GetPlayerRespawnPosition();

    //        int possessedEnemyToRespawn = GameManager.Instance.GetPossessedEnemyToRespawn();
    //        if (possessedEnemyToRespawn != -1)
    //        {
    //            gameObjectsToRespawn[possessedEnemyToRespawn].GetComponent<HostAbsorption>().interactable = false;
    //            StartCoroutine(ApplyPossessionRoutine(gameObjectsToRespawn[possessedEnemyToRespawn]));
    //        }

    //    }

    //    if (GameManager.Instance.GetPlayerRespawnPosition() != Vector3.zero && GameManager.Instance.GetPossessedEnemyToRespawn() == -1)
    //    {
    //        GameManager.Instance.GetPlayerController().gameObject.transform.position = GameManager.Instance.GetPlayerRespawnPosition();
    //    }
    //}
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
        GameManager.Instance.soundtrackManager.StopAllSFX();
        GameManager.Instance.soundtrackManager.ResetSoundtrack();

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
        onLevelRestart?.Invoke();
        //GameManager.Instance.SetLastLevelPlayed(level);
        
    }

    //IEnumerator ApplyPossessionRoutine(GameObject go)
    //{
    //    yield return null;
    //    Vector3 checkpointRespawn = GameManager.Instance.GetPlayerRespawnPosition();
    //    if (checkpointRespawn != Vector3.zero)
    //    {
    //        go.transform.position = checkpointRespawn;
    //    }
    //    go.GetComponent<HostAbsorption>().ApplyPossessionWithNoEffects(go);
    //}
}
