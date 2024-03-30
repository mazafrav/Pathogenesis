using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    [SerializeField]
    public float transitionTime = 0.7f;
    
    public void StartLoadingLevel(int level)
    {
        StartCoroutine(LoadLevel(level));
    }

    public void RestartLevel()
    {
        StartLoadingLevel(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator LoadLevel(int level)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(level);
    }
}
