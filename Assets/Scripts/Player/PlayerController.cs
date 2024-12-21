using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private VisualEffect absortionRangeVfx;

    [SerializeField]
    private string inAbsorptionRangeEventPath = "event:/SFX/Player/Player Possession Proximity";
    private FMOD.Studio.EventInstance inAbsorptionRangeEventInstance;
    private FMODUnity.StudioEventEmitter proximityEmitter;

    [SerializeField]
    private ParticleSystem deathEffect;

    public delegate void OnHostDeath();
    public OnHostDeath onHostDeath;
    public delegate void OnVirusDeath();
    public OnVirusDeath onVirusDeath;
    public delegate void OnPossesion(HostLocomotion locomotion);
    public OnPossesion onPossession;


    [SerializeField]
    public HostLocomotion locomotion;
    [SerializeField]
    private float initialTimeWithDisabledInput = 0.7f;

    private float deltaX = 0.0f, deltaY = 0.0f;

    public ShootingComponent shootingComponent { get; set; } = null;
    public HostAbsorption AbsorbableHostInRange { get; private set; } = null;

    public bool HasDisabledControls { get; set; } = false;
    private Vector3 mousePos;

    private Queue<string> inputBuffer;
    [SerializeField]
    private float inputBufferTime = 0.5f;

    public bool isPossessing { get; set; } = false;
    private bool doOnce = false;
    private PlayerInputActions playerInputActions;

    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Disable();
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Jump.canceled += JumpButtomUp;
        playerInputActions.Player.Attack.performed += Attack;
        playerInputActions.Player.Attack.canceled += CancelAttack;
        playerInputActions.Player.PauseMenu.performed += PauseMenu;

        inputBuffer = new Queue<string>(5);

        StartCoroutine(EnableInput());

        proximityEmitter = absortionRangeVfx.gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
        //inAbsorptionRangeEventInstance = FMODUnity.RuntimeManager.CreateInstance(inAbsorptionRangeEventPath);
    }

    public float GetDeltaX() { return deltaX; }
    public float GetDeltaY() { return deltaY; }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (!isPossessing)
            {
                if (doOnce)
                {
                    GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    doOnce = false;
                }

                deltaX = playerInputActions.Player.Movement.ReadValue<Vector2>().x;
                deltaY = playerInputActions.Player.Movement.ReadValue<Vector2>().y;

                locomotion.Aim(mousePos);

                if (AbsorbableHostInRange != null)
                {
                    UpdateAbsortionVfxDirection();
                }
            }
            else
            {
                GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
                doOnce = true;
            }

            //Attacking with electric enemy
            ElectricLocomotion electricLocomotion = GetComponentInParent<ElectricLocomotion>();
            if (electricLocomotion)
            {
                if ((GameManager.Instance.IsThereAGamepadConnected && Gamepad.current.rightShoulder.isPressed) || Input.GetMouseButton(0))
                {
                    locomotion.Attack(mousePos);
                }
                else if (electricLocomotion.inAttackRange && electricLocomotion.currentRemainingShockTime > 0.0f && (GameManager.Instance.IsThereAGamepadConnected && !Gamepad.current.rightShoulder.isPressed) || !Input.GetMouseButton(0))
                {
                    //We wait a bit to deactivate the shock
                    electricLocomotion.currentRemainingShockTime -= Time.deltaTime;

                    if (electricLocomotion.currentRemainingShockTime <= 0.0f)
                    {
                        locomotion.DeactivateAttack();
                        electricLocomotion.inAttackRange = false;
                    }
                }
            }

            if (proximityEmitter.IsPlaying())
            {
                if (AbsorbableHostInRange == null)
                {
                    StopAbsorbableSFX();
                }
            }


            // INPUT BUFFER ANALYSIS
            if (inputBuffer.Count > 0)
            {
                switch (inputBuffer.Peek())
                {
                    case "jump":
                    {

                            if (locomotion.CanJump())
                            {
                                locomotion.Jump(deltaX);
                                inputBuffer.Dequeue();
                            }
                            break;
                    }
                    case "attack":
                    {
                        if (locomotion.IsAttackReady())
                        {
                            locomotion.Attack(mousePos);
                            inputBuffer.Dequeue();
                        }
                        break;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        locomotion.Move(deltaX, deltaY);

        mousePos = GameManager.Instance.IsThereAGamepadConnected ? playerInputActions.Player.Aim.ReadValue<Vector2>() : Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (shootingComponent)
        {
            shootingComponent.Aim(mousePos);
        }
    }


    #region PlayerInputActions

    public void UnregisterPlayerInputActions()
    {
        playerInputActions.Player.Jump.performed -= Jump;
        playerInputActions.Player.Jump.canceled -= JumpButtomUp;
        playerInputActions.Player.Attack.performed -= Attack;
        playerInputActions.Player.Attack.performed -= CancelAttack;
        playerInputActions.Player.PauseMenu.performed -= PauseMenu;
    }

    void RemoveAction()
    {
        if (inputBuffer.Count > 0)
        {
            inputBuffer.Dequeue();
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {

        if (GameManager.Instance.canPlayerProcessInput)
        {
            Debug.Log("Jump");
            inputBuffer.Enqueue("jump");
            Invoke("RemoveAction", inputBufferTime);
        }
    }

    private void JumpButtomUp(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.canPlayerProcessInput)
        {
            locomotion.JumpButtonUp();
            //inputBuffer.Enqueue("jumpUp");
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.canPlayerProcessInput)
        {
            //locomotion.Attack(mousePos);
            inputBuffer.Enqueue("attack");
            Invoke("RemoveAction", inputBufferTime);
        }
    }

    private void CancelAttack(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.canPlayerProcessInput)
        {
            locomotion.CancelAttack();
        }
    }

    private void PauseMenu(InputAction.CallbackContext context)
    {
        //Debug.Log("Pausa");

        bool isSettingsOn = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals("SettingsMenu"))
            {
                isSettingsOn = true;
                break;
            }
        }
        if (!isSettingsOn)
        {
            GameManager.Instance.PauseGame();
        }
    }
    #endregion

    public void OnEnterAbsorbableRange(HostAbsorption host)
    {
        AbsorbableHostInRange = host;
        absortionRangeVfx.SetVector3("Direction", (host.transform.position - playerBody.transform.position).normalized);
        absortionRangeVfx.Play();


        proximityEmitter.Play();

        //if (!proximityEmitter.IsPlaying())
        //{
        //    proximityEmitter.Play();
        //}
    }

    public void UpdateAbsortionVfxDirection()
    {
        absortionRangeVfx.SetVector3("Direction", (AbsorbableHostInRange.transform.position - playerBody.transform.position).normalized);
    }

    public void OnLeaveAbsorbableRange()
    {
        AbsorbableHostInRange = null;
        absortionRangeVfx.Stop();
    }

    public void StopAbsorbableSFX()
    {
        proximityEmitter.Stop();
        //proximityEmitter.EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);  
    }

    public void PlayerBodyDeath()
    {
        Instantiate(deathEffect, playerBody.transform.position, playerBody.transform.rotation);
        onVirusDeath?.Invoke();
        DisablePlayerBody();
    }

    public void DisablePlayerBody()
    {
        if (playerBody)
        {
            playerBody.SetActive(false);
        }
    }

    public void EnablePlayerBody()
    {
        if (playerBody)
        {
            playerBody.SetActive(true);
            //GameManager.Instance.SetMusicSelectionIndex(1);
        }

    }

    public GameObject GetPlayerBody() { return playerBody; }

    public PlayerInputActions GetPlayerIAs() { return playerInputActions; }

    private IEnumerator EnableInput()
    {
        yield return new WaitForSeconds(initialTimeWithDisabledInput); 
        playerInputActions.Enable();
    } 

    public void OnPossesionEvent(HostLocomotion locomotion)
    {
        onPossession?.Invoke(locomotion);
    }
}
