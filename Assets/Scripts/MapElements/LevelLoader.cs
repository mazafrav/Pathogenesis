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

    public void StartLoadingLevel(int level)
    {
        StartCoroutine(LoadLevel(level));
    }

    public void RestartLevel()
    {      
        GameManager.Instance.GetPlayerController().UnregisterPlayerInputActions();
        GameManager.Instance.GetPlayerController().GetPlayerIAs().Disable();
        StartLoadingLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void CheckRespawn()
    {
        GameManager.Instance.soundtrackManager.StopAllSFX();
        GameManager.Instance.soundtrackManager.ResetSoundtrack();

        //if (GameManager.Instance.GetRespawnValues().Count > 0 || GameManager.Instance.GetPlayerRespawnPosition() != Vector3.zero)
        //{
        //    GameManager.Instance.GetPlayerController().UnregisterPlayerInputActions();
        //    StartLoadingLevel(SceneManager.GetActiveScene().buildIndex);
        //}
        //else
        //{
            RestartLevel();
        //}

    }

    IEnumerator LoadLevel(int level)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(level);
        onLevelRestart?.Invoke();    
    }

 
}
