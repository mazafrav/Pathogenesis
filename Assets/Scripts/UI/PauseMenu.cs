using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
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
        SceneManager.LoadScene("MainMenu");
        //SceneManager.UnloadSceneAsync("Pause")
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("PauseMenu");
    }
}
