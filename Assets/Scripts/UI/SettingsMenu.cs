using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private GameObject eventSystem;
    private GameObject eventSystem1;
    private List<Resolution> filteredResolutions;

    private void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystemMainMenu");
        eventSystem1 = GameObject.FindGameObjectWithTag("EventSystemSettings");

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        int minWidth = 800;
        int minHeight = 600;
        float targetAspectRatio = 16f / 9f;

        // Filter resolutions based on criteria
        filteredResolutions = new List<Resolution>();
        foreach (Resolution res in Screen.resolutions)
        {
            // Filter by minimum resolution size and aspect ratio
            if (res.width >= minWidth && res.height >= minHeight &&
                Mathf.Approximately((float)res.width / res.height, targetAspectRatio))
            {
                filteredResolutions.Add(res);
            }
        }


        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string option = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            options.Add(option);

            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
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
                eventSystem1.gameObject.GetComponent<InputSystemUIInputModule>().enabled = false;
                //Main menu
                eventSystem.gameObject.GetComponent<EventSystem>().enabled = true;
                eventSystem.gameObject.GetComponent<InputSystemUIInputModule>().enabled = true;
                break;
            }
        }

        if (!isMainMenuOn)
        {
            //GameObject eventSys = GameObject.FindGameObjectWithTag("EventSystemLevel");
            //eventSys.GetComponent<EventSystem>().enabled = false;
            //eventSys.GetComponent<InputSystemUIInputModule>().enabled = false;

            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
    }
}
