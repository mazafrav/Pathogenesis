using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    void Start()
    {

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

                deltaX = Input.GetAxisRaw("Horizontal");
                deltaY = Input.GetAxisRaw("Vertical");

                locomotion.Aim(mousePos);

                if (Input.GetKeyDown(KeyCode.W))
                {
                    locomotion.Jump(deltaX);
                }
                else if (Input.GetKeyUp(KeyCode.W))
                {
                    locomotion.JumpButtonUp();
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    locomotion.Attack(mousePos);
                }
                //else if (Input.GetKeyDown(KeyCode.F))
                //{
                //    locomotion.Unpossess();
                //}
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.PauseGame();
        }
    }

    private void FixedUpdate()
    {
        locomotion.Move(deltaX, deltaY);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(shootingComponent)
        {
            shootingComponent.Aim(mousePos);
        }
    }

    public void DisablePlayerBody()
    {
        if (playerBody)
        {
            playerBody.SetActive(false);
        }
    }

    public void PlayerBodyDeath()
    {
        Instantiate(deathEffect, playerBody.transform.position, playerBody.transform.rotation);
        DisablePlayerBody();
    }
    public void EnablePlayerBody()
    {
        if (playerBody)
        {
            playerBody.SetActive(true);
        }

    }

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

    public GameObject GetPlayerBody() { return playerBody; }
}
