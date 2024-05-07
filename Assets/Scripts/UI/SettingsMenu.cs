using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{    
    public void Back()
    {
        SceneManager.UnloadSceneAsync("SettingsMenu");
        bool isMainMenuOn = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals("MainMenu"))
            {
                isMainMenuOn = true;
                break;
            }
        }

        if (!isMainMenuOn)
        {
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
    }
}
