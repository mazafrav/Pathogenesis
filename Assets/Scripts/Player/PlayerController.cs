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
    public HostLocomotion locomotion;
    private float deltaX = 0.0f, deltaY = 0.0f;

    public ShootingComponent shootingComponent;
    //public float DeltaX { set { deltaX = value; } }
    public HostAbsorption AbsorbableHostInRange { get; private set; } = null;

    void Start()
    {

    }

    public float GetDeltaX() { return deltaX; }
    public float GetDeltaY() { return deltaY; }

    void Update()
    {
        deltaX = Input.GetAxisRaw("Horizontal");
        deltaY = Input.GetAxisRaw("Vertical");

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
            if (!locomotion.GetType().Name.Equals("RangedLocomotion"))
            {
                locomotion.Attack();
            }
            else
            {
                locomotion.Attack(shootingComponent.mousePosition);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            locomotion.Unpossess();
        }
        if(AbsorbableHostInRange != null)
        {
            UpdateAbsortionVfxDirection();
        }
    }

    private void FixedUpdate()
    {
        locomotion.Move(deltaX, deltaY);
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

    public void OnEnterAbsorbableRange(HostAbsorption host)
    {
        AbsorbableHostInRange = host;
        absortionRangeVfx.SetVector3("Direction", (host.transform.position - playerBody.transform.position).normalized);
        absortionRangeVfx.Play();
        Debug.Log("IN RANGE: " + host.name);
    }

    public void UpdateAbsortionVfxDirection()
    {
        absortionRangeVfx.SetVector3("Direction", (AbsorbableHostInRange.transform.position - playerBody.transform.position).normalized);
    }

    public void OnLeaveAbsorbableRange()
    {
        AbsorbableHostInRange = null;
        absortionRangeVfx.Stop();
        Debug.Log("NOT IN RANGE");
    }

    public GameObject GetPlayerBody() { return playerBody; }
}
