using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private LevelLoader levelLoader;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private PlayerController playerController;
    private PlayerLocomotion playerLocomotion;

    private GameObject levelEventSystem;

    public bool isPaused = false;

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
    }

    private void Update()
    {
        //We check if there is a gamepad connected
        IsThereAGamepadConnected = Input.GetJoystickNames()[0].Length > 0;    
        
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
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
        else
        {           
            levelEventSystem.SetActive(true);
            processInputTimer = 0.1f;
            Time.timeScale = 1.0f;
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
    }
}
