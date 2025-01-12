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
    private GameData gameData;

    private void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystemMainMenu");

        gameData = GameManager.Instance.GetSaveSystem().GetGameData();
       
        if(gameData.GetCurrentLevelName() != "")
        {
            eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = continueButton;
        }
        else
        {
            eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = playButton;
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;

        GameManager.Instance.GetSaveSystem().DeleteGameData();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;

        GameData gameData = GameManager.Instance.GetSaveSystem().GetGameData();
        SceneManager.LoadScene(gameData.GetCurrentLevelName());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        
        if (gameData.GetCurrentLevelName() == "")
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
            if(gameData.GetCurrentLevelName() != "")
            {             
                mainMenu.SetActive(false);
                continueMenu.SetActive(true);
            }
            else 
            {
                mainMenu.SetActive(true);
                continueMenu.SetActive(false);
            }
        }
    }
}
