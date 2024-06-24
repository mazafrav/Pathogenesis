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
    private ParticleSystem deathEffect;

    [SerializeField]
    public HostLocomotion locomotion;
    private float deltaX = 0.0f, deltaY = 0.0f;

    public ShootingComponent shootingComponent;
    public HostAbsorption AbsorbableHostInRange { get; private set; } = null;

    public bool HasDisabledControls { get; set; } = false;
    private Vector3 mousePos;

    Queue<string> inputBuffer;
    [SerializeField]
    private float inputBufferTime = 0.5f;

    public bool isPossessing = false;
    private bool doOnce = false;
    PlayerInputActions playerInputActions;

    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Jump.canceled += JumpButtomUp;
        playerInputActions.Player.Attack.performed += Attack;
        playerInputActions.Player.PauseMenu.performed += PauseMenu;

        inputBuffer = new Queue<string>(5);
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
                    GetComponentInChildren<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    GetComponentInChildren<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
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
                GetComponentInChildren<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
                doOnce = true;
            }

            // INPUT BUFFER ANALYSIS
            if (inputBuffer.Count > 0)
            {
                switch(inputBuffer.Peek())
                {
                    case "jump":
                        if (locomotion.groundChecker.isGrounded)
                        {
                            locomotion.Jump(deltaX);
                            inputBuffer.Dequeue();
                        }
                        break;
                    case "attack":
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

    private void FixedUpdate()
    {
        locomotion.Move(deltaX, deltaY);

        mousePos = GameManager.Instance.IsThereAGamepadConnected ? playerInputActions.Player.Aim.ReadValue<Vector2>() : Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(shootingComponent)
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
            inputBuffer.Enqueue("jump");
            Invoke("RemoveAction", inputBufferTime);
        }
    }

    private void JumpButtomUp(InputAction.CallbackContext context)
    {
        locomotion.JumpButtonUp();
        //inputBuffer.Enqueue("jumpUp");
    }

    private void Attack(InputAction.CallbackContext context)
    {
        //locomotion.Attack(mousePos);
        inputBuffer.Enqueue("attack");
        Invoke("RemoveAction", inputBufferTime);
    }

    private void PauseMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Pausa");

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

    public void PlayerBodyDeath()
    {
        Instantiate(deathEffect, playerBody.transform.position, playerBody.transform.rotation);
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
            GameManager.Instance.SetMusicSelectionIndex(1);
        }

    }

    public GameObject GetPlayerBody() { return playerBody; }
}
