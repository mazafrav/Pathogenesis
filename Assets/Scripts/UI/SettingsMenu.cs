using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private GameObject eventSystem;
    private GameObject eventSystem1;

    private void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystemMainMenu");
        eventSystem1 = GameObject.FindGameObjectWithTag("EventSystemSettings");

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
     
    }

    private void Update()
    {
        if (Input.GetButtonDown("PauseMenu"))
        {
            Back();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
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
