using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject eventSystem;

    private void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystemPause");       
    }

    public void ResumeGame()
    {
        GameManager.Instance.PauseGame();
    }

    public void ResetLevel()
    {
        GameManager.Instance.ResetLevelFromPause();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
        //SceneManager.UnloadSceneAsync("Pause")
    }

    public void OpenSettings()
    {
        eventSystem.GetComponent<EventSystem>().enabled = false;
        eventSystem.GetComponent<StandaloneInputModule>().enabled = false;

        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("PauseMenu");
    }
}
