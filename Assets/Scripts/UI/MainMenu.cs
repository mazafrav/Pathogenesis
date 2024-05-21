using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    private GameObject eventSystem;

    private void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystemMainMenu");
    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
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
        eventSystem.gameObject.GetComponent<EventSystem>().enabled = false;
        eventSystem.gameObject.GetComponent<InputSystemUIInputModule>().enabled = false;
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
