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
    public enum GamepadType { Dualshock, XboxController };

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private LevelLoader levelLoader;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    public SoundtrackManager soundtrackManager { get; private set; }

    private GameObject levelEventSystem;
    private int pausedMusicSelection = 1;
    private float processInputTimer = 0.0f;

    private List<GameObject> gameObjectsToRespawn = new List<GameObject>();
    private Dictionary<int, Vector3> respawnValues = new Dictionary<int, Vector3>();
    private Vector3 playerRespawnPosition = Vector3.zero;
    private int possessedEnemyToRespawn = -1;

    public bool isPaused { get; set; } = false;
    public bool canPlayerProcessInput { get; set; } = true;
    public bool IsThereAGamepadConnected { get; private set; }
    public GamepadType gamepadType { get; private set; }
    public static GameManager Instance { get; private set; }

    public delegate void OnPlayerSet();
    public OnPlayerSet onPlayerSet;

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
        soundtrackManager = GetComponentInChildren<SoundtrackManager>();
    }

    private void Update()
    {
        //We check if there is a gamepad connected
        IsThereAGamepadConnected = Gamepad.all.Count > 0;

        if (IsThereAGamepadConnected)
        {
            if (Gamepad.current is DualShockGamepad)
            {
                gamepadType = GamepadType.Dualshock;
            }
            else if (Gamepad.current is XInputController)
            {
                gamepadType = GamepadType.XboxController;
            }
        }


        //We add a delay to the player input when we resume the game from the pause menu
        if (processInputTimer > 0)
        {
            processInputTimer -= Time.deltaTime;

            if (processInputTimer <= 0)
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
            onPlayerSet.Invoke();
        }
        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInParent<CinemachineVirtualCamera>();
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

    public GameObject GetPlayer()
    { return player; }

    public GameObject GetPlayerMovementBody()
    {
        if (player.GetComponentInChildren<HostLocomotion>().GetType() != typeof(PlayerLocomotion))
        {
            return player.GetComponentInChildren<HostLocomotion>().gameObject;
        }

        return player.GetComponentInChildren<Rigidbody2D>().gameObject;
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
            soundtrackManager.ApplyPauseSnapshot();
            canPlayerProcessInput = false;
            Time.timeScale = 0.0f;
            //pausedMusicSelection = soundtrackManager.GetSelectionIndex();
            //soundtrackManager.SetSelectionIndex(2);

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
            soundtrackManager.StopPausepSnapshot();
            processInputTimer = 0.1f;
            Time.timeScale = 1.0f;
            SceneManager.UnloadSceneAsync("PauseMenu");
            //soundtrackManager.SetSelectionIndex(pausedMusicSelection);
        }
    }

    //public void SetMusicSelectionIndex(int index)
    //{
    //    soundtrackManager.SetSelectionIndex(index);
    //}

    public List<GameObject> GetGameObjectsToRespawn()
    {
        return gameObjectsToRespawn;
    }

    public void AddGameObjectToRespawn(GameObject go)
    {
        if (gameObjectsToRespawn.Contains(go))
        {
            return;
        }
        gameObjectsToRespawn.Add(go);
    }

    public void AddGameObjectToRespawn(List<GameObject> list)
    {
        gameObjectsToRespawn = list;
    }

    public void ResetGameObjectsToRespawn()
    {
        gameObjectsToRespawn.Clear();
    }

    public Dictionary<int, Vector3> GetRespawnValues()
    {
        return respawnValues;
    }
    public void AddRespawnValue(int go, Vector3 position)
    {
        if (respawnValues.ContainsKey(go))
        {
            return;
        }
        respawnValues.Add(go, position);
    }

    public void ResetRespawnValues()
    {
        respawnValues.Clear();
    }

    public void ResetPlayerRespawnPosition()
    {
        playerRespawnPosition = Vector3.zero;
    }

    public Vector3 GetPlayerRespawnPosition()
    {
        return playerRespawnPosition;
    }

    public void SetPlayerRespawnPosition(Vector3 position)
    {
        playerRespawnPosition = position;
    }

    public void ClearRespawn()
    {
        ResetGameObjectsToRespawn();
        ResetRespawnValues();
        ResetPlayerRespawnPosition();
        ResetPossessedEnemyToRespawn();
    }

    public int GetPossessedEnemyToRespawn()
    {
        return possessedEnemyToRespawn;
    }

    public void SetPossessedEnemyToRespawn(int i)
    {
        possessedEnemyToRespawn = i;
    }

    public void ResetPossessedEnemyToRespawn()
    {
        possessedEnemyToRespawn = -1;
    }

    public void SetLastLevelPlayed(int index)
    {
        PlayerPrefs.SetInt("LastLevel", index);
        PlayerPrefs.Save();
    }
}
