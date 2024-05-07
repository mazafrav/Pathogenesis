using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit from menu");
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (SceneManager.sceneCount == 1)
        {
            mainMenu.SetActive(true);
        }
    }
}
