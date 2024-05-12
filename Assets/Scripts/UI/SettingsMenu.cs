using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    private GameObject eventSystem;
    private GameObject eventSystem1;

    private void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystemMainMenu");
        eventSystem1 = GameObject.FindGameObjectWithTag("EventSystemSettings");
    }

    public void Back()
    {
        SceneManager.UnloadSceneAsync("SettingsMenu");
        bool isMainMenuOn = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals("MainMenu"))
            {
                isMainMenuOn = true;
                //Settings
                eventSystem1.gameObject.GetComponent<EventSystem>().enabled = false;
                eventSystem1.gameObject.GetComponent<StandaloneInputModule>().enabled = false;
                //Main menu
                eventSystem.gameObject.GetComponent<EventSystem>().enabled = true;
                eventSystem.gameObject.GetComponent<StandaloneInputModule>().enabled = true;
                break;
            }
        }

        if (!isMainMenuOn)
        {
            //GameObject eventSys = GameObject.FindGameObjectWithTag("EventSystemLevel");
            //eventSys.GetComponent<EventSystem>().enabled = false;
            //eventSys.GetComponent<StandaloneInputModule>().enabled = false;

            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
    }
}
