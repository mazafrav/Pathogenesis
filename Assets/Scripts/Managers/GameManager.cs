using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private LevelLoader levelLoader;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private DynamicMusicController musicController;

    private PlayerController playerController;
    private PlayerLocomotion playerLocomotion;

    private GameObject levelEventSystem;

    public bool isPaused = false;
    private int pausedMusicSelection = 1;

    public bool canPlayerProcessInput { get; private set; } = true;
    private float processInputTimer = 0.0f;

    public bool IsThereAGamepadConnected {  get; private set; }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
       levelEventSystem = GameObject.Find("EventSystem");
       musicController = DynamicMusicController.instance;
    }

    private void Update()
    {
        //We check if there is a gamepad connected
        IsThereAGamepadConnected = Gamepad.all.Count > 0;
        //if(Input.GetJoystickNames().Length > 0)
        //{
        //     IsThereAGamepadConnected = Input.GetJoystickNames()[0].Length > 0;    
        //}
        
        //We add a delay to the player input when we resume the game from the pause menu
        if(processInputTimer > 0)
        {
            processInputTimer -= Time.deltaTime;

            if(processInputTimer <=0)
            {
                canPlayerProcessInput = true;          
            }
        }
    }

    public PlayerController GetPlayerController()
    {
        return player.GetComponent<PlayerController>();
    }

    public PlayerLocomotion GetPlayerLocomotion()
    {
        return player.GetComponent<PlayerLocomotion>();
    }

    public LevelLoader GetLevelLoader()
    {
        return levelLoader;
    }

    public CinemachineVirtualCamera GetCamera() 
    { 
        return virtualCamera;
    }

    public void ResetLevelFromPause()
    {
        PauseGame();
        GameManager.Instance.GetLevelLoader().RestartLevel();
    }

    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {         
            levelEventSystem.SetActive(false);
            canPlayerProcessInput = false;
            Time.timeScale = 0.0f;
            pausedMusicSelection = musicController.GetSelectionIndex();
            musicController.SetSelectionIndex(2);

            bool isPauseOn = false;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name.Equals("PauseMenu"))
                {
                    isPauseOn = true;
                    break;
                }
            }
            if (!isPauseOn)
            {
                SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
            }
        }
        else
        {           
            levelEventSystem.SetActive(true);
            processInputTimer = 0.1f;
            Time.timeScale = 1.0f;
            SceneManager.UnloadSceneAsync("PauseMenu");
            musicController.SetSelectionIndex(pausedMusicSelection);
        }
    }

    public void SetMusicSelectionIndex(int index)
    {
        musicController.SetSelectionIndex(index);
    }
}
