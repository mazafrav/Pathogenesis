using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
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
        GameManager.Instance.GetSaveSystem().GetGameData().DeleteLevelData(SceneManager.GetActiveScene().name);
        GameManager.Instance.GetPlayerController().UnregisterPlayerInputActions();
        GameManager.Instance.ResetLevelFromPause();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.GetPlayerController().UnregisterPlayerInputActions();
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        eventSystem.GetComponent<EventSystem>().enabled = false;
        eventSystem.GetComponent<InputSystemUIInputModule>().enabled = false;

        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("PauseMenu");
    }
}
