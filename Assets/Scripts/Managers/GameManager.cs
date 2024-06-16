using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.XInput;
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
    private int pausedMusicSelection = 1;
    private float processInputTimer = 0.0f;

    public bool isPaused { get; set; } = false;
    public bool canPlayerProcessInput { get; set; } = true;
    public bool IsThereAGamepadConnected {  get; private set; }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Instance.SetInfo();
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Instance.SetInfo(); //This is neccessary if we start in any level
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
       musicController = GetComponentInChildren<DynamicMusicController>();      
    }

    private void Update()
    {
        //We check if there is a gamepad connected
        IsThereAGamepadConnected = Gamepad.all.Count > 0;
        
        //if(IsThereAGamepadConnected)
        //{
        //    if(Gamepad.current is DualShockGamepad)
        //    {
        //        Debug.Log("DualShock connected");
        //    }
        //    else if(Gamepad.current is XInputController) 
        //    {
        //        Debug.Log("XBOX gamepad connected");
        //    }
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

    //Here we set info for the game manager such as player, camera...
    public void SetInfo()
    {
        isPaused = false;
        canPlayerProcessInput = true;

        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }
        if (levelEventSystem == null)
        {
            levelEventSystem = GameObject.FindGameObjectWithTag("EventSystemLevel");
        }
        if (levelLoader == null)
        {
            levelLoader = GameObject.FindObjectOfType<LevelLoader>();
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
        levelLoader.RestartLevel();
    }

    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            levelEventSystem.GetComponent<EventSystem>().enabled = false;
            levelEventSystem.GetComponent<InputSystemUIInputModule>().enabled = false;
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
            levelEventSystem.GetComponent<EventSystem>().enabled = true;
            levelEventSystem.GetComponent<InputSystemUIInputModule>().enabled = true;
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
