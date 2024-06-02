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

                #region OldInput
                //deltaX = Input.GetAxisRaw("Horizontal");
                //deltaY = Input.GetAxisRaw("Vertical");


                //if (Input.GetButtonDown("Jump") && GameManager.Instance.canPlayerProcessInput)
                //{
                //    locomotion.Jump(deltaX);
                //}
                //else if (Input.GetButtonUp("Jump"))
                //{
                //    locomotion.JumpButtonUp();
                //}
                //else if (Input.GetButtonDown("Attack"))
                //{              
                //    locomotion.Attack(mousePos);
                //}
                //else if (Input.GetKeyDown(KeyCode.F))
                //{
                //    locomotion.Unpossess();
                //}
                #endregion

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
        }

        #region OldInput
        //if (Input.GetButtonDown("PauseMenu"))
        //{
        //    bool isSettingsOn = false;
        //    for (int i = 0; i < SceneManager.sceneCount; i++)
        //    {
        //        if (SceneManager.GetSceneAt(i).name.Equals("SettingsMenu"))
        //        {
        //            isSettingsOn = true;
        //            break;
        //        }
        //    }
        //    if (!isSettingsOn)
        //    {
        //        GameManager.Instance.PauseGame();
        //    }
        //}
        #endregion
    }

    private void FixedUpdate()
    {
        locomotion.Move(deltaX, deltaY);

        mousePos = GameManager.Instance.IsThereAGamepadConnected ? /*new Vector2(Input.GetAxisRaw("AimX"), Input.GetAxisRaw("AimY"))*/  playerInputActions.Player.Aim.ReadValue<Vector2>() : Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
    private void Jump(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.canPlayerProcessInput)
        {
            locomotion.Jump(deltaX);
        }
    }

    private void JumpButtomUp(InputAction.CallbackContext context)
    {
        locomotion.JumpButtonUp();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        locomotion.Attack(mousePos);
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
        }

    }

    public GameObject GetPlayerBody() { return playerBody; }
}
