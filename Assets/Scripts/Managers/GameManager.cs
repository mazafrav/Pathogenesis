using Cinemachine;
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
    //private GameObject virusBody;
    private PlayerController playerController;
    private PlayerLocomotion playerLocomotion;

    [SerializeField]
    private LevelLoader levelLoader;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    public SoundtrackManager soundtrackManager { get; private set; }

    private GameObject levelEventSystem;
    private float processInputTimer = 0.0f;

    public bool isPaused { get; set; } = false;
    public bool canPlayerProcessInput { get; set; } = true;
    public bool IsThereAGamepadConnected { get; private set; }
    public GamepadType gamepadType { get; private set; }
    public static GameManager Instance { get; private set; }

    public delegate void OnPlayerSet();
    public OnPlayerSet onPlayerSet;

    [SerializeField]
    private int targetFrameRate = 144;
    [SerializeField]
    bool limitFps = false;

    private SaveSystem saveSystem;

    public SaveSystem GetSaveSystem() => saveSystem;

    private void Awake()
    {
#if UNITY_STANDALONE && !UNITY_EDITOR
            Mouse.current.WarpCursorPosition(Vector2.zero);
            Cursor.visible = false;
#endif

        if (Instance != null)
        {
            Instance.SetInfo();
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Instance.SetInfo(); //This is neccessary if we start in any level
        saveSystem = new SaveSystem();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        soundtrackManager = GetComponentInChildren<SoundtrackManager>();
        if (limitFps)
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }

    private void Update()
    {
        //We check if there is a gamepad connected
        IsThereAGamepadConnected = Gamepad.all.Count > 0;

        if (IsThereAGamepadConnected)
        {
            // Restrict input to gamepad only
            InputSystem.DisableDevice(Mouse.current);
            InputSystem.DisableDevice(Keyboard.current);

#if UNITY_STANDALONE && !UNITY_EDITOR
            Mouse.current.WarpCursorPosition(Vector2.zero);
            Cursor.visible = false;
#endif


            if (Gamepad.current is DualShockGamepad)
            {
                gamepadType = GamepadType.Dualshock;
            }
            else if (Gamepad.current is XInputController)
            {
                gamepadType = GamepadType.XboxController;
            }
        }
        else
        {
            InputSystem.EnableDevice(Mouse.current);
            InputSystem.EnableDevice(Keyboard.current);
            Cursor.visible = true;
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

            if(player)
            {
                playerLocomotion = player.GetComponent<PlayerLocomotion>();
                onPlayerSet?.Invoke();
            }
        }

        if (player)
        {
            playerController = player.GetComponentInChildren<PlayerController>();
        }

        if (player)
        {
            playerLocomotion = player.GetComponent<PlayerLocomotion>();           
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
    { 
        return player; 
    }

    public void SetPlayer(GameObject player) { this.player = player; }

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
        return playerController;
    }

    public PlayerLocomotion GetPlayerLocomotion()
    {
        return playerLocomotion;
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
        }
    }
}
