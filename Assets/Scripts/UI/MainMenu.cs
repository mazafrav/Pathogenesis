using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject mainMenuWithContinue;
    [SerializeField]
    private GameObject continueMenu;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject playButton;

    private GameObject eventSystem;


    private void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystemMainMenu");

        if (PlayerPrefs.GetInt("LastLevel", 0) <= 1 )
        {
            eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = playButton;
        }
        else
        {
            eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = continueButton;
        }

    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.SetLastLevelPlayed(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;
        int scene = PlayerPrefs.GetInt("LastLevel", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        if (PlayerPrefs.GetInt("LastLevel", 0) <= 1)
        {
            mainMenu.SetActive(false);
        }
        else
        {
            mainMenuWithContinue.SetActive(false);
        }

        eventSystem.gameObject.GetComponent<EventSystem>().enabled = false;
        eventSystem.gameObject.GetComponent<InputSystemUIInputModule>().enabled = false;
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (SceneManager.sceneCount == 1)
        {
            if (PlayerPrefs.GetInt("LastLevel", 0) <= 1)
            {
                mainMenu.SetActive(true);
                continueMenu.SetActive(false);
            }
            else
            {
                mainMenu.SetActive(false);
                continueMenu.SetActive(true);
            }
        }
    }
}
