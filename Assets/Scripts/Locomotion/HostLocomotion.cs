using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HostLocomotion : MonoBehaviour
{
    [SerializeField]
    protected PossessingParameters possessingParameters;

    public LocomotionEventNames locomotionEventNames;

    protected FMOD.Studio.EventInstance jumpEventInstance;
    protected FMOD.Studio.EventInstance landEventInstance;
    protected FMOD.Studio.EventInstance attackEventInstance;


    [Header("Movement")]
    [SerializeField]
    protected float moveSpeed = 5.0f;

    [Header("Jump")]
    [SerializeField]
    protected float jumpHeight = 2;
    [SerializeField]
    protected float jumpDistance = 3;
    [SerializeField]
    protected float coyoteTime = 0.1f;
    protected float coyoteTimeCounter;
    [SerializeField]
    protected float jumpBufferTime = 0.1f;
    protected float jumpBufferCounter;

    [SerializeField]
    public GroundChecker groundChecker;

    protected float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;

    private DamageControl damageControl = null;
    private Collider2D hostCollider;

    public Rigidbody2D rb2D { protected set; get; } = null;

    private void Awake()
    {
        if (locomotionEventNames.JumpEventName != "")
            jumpEventInstance = FMODUnity.RuntimeManager.CreateInstance(locomotionEventNames.GenericEventsPath + locomotionEventNames.JumpEventName);
        if (locomotionEventNames.LandEventName != "")
            landEventInstance = FMODUnity.RuntimeManager.CreateInstance(locomotionEventNames.GenericEventsPath + locomotionEventNames.LandEventName);
        if (locomotionEventNames.AttackEventName != "")
            attackEventInstance = FMODUnity.RuntimeManager.CreateInstance(locomotionEventNames.GenericEventsPath + locomotionEventNames.AttackEventName);
    }

    public abstract void Jump(float deltaX);
    public abstract void JumpCancel();
    public void JumpButtonUp() { coyoteTimeCounter = 0; JumpCancel();  }
    public abstract void Move(float deltaX, float deltaY=0);
    public abstract void Attack(Vector3 target = default);
    public abstract void CancelAttack();
    public abstract bool IsAttackReady();
    public abstract void ResetAttack();

    public abstract void Aim(Vector3 target = default);
    public virtual void DeactivateAttack()
    {

    }
    public virtual void Unpossess()
    {
        if (this.GetType() != typeof(PlayerLocomotion))
        {
            damageControl = GetComponentInChildren<DamageControl>();
            hostCollider = GetComponentInChildren<Collider2D>();
            damageControl.Damage(hostCollider);
            CinemachineVirtualCamera cinemachineVirtualCamera = GameManager.Instance.GetCamera();
            if (cinemachineVirtualCamera != null)
            {
                cinemachineVirtualCamera.Follow = GameManager.Instance.GetPlayerController().GetPlayerBody().transform;
            }
        }    
    }

    public virtual void SetPossessingParameters()
    {
        moveSpeed = possessingParameters.moveSpeed;
        jumpHeight = possessingParameters.jumpHeight;
        jumpDistance = possessingParameters.jumpDistance;
    }


    public virtual void SetMoveSpeed(float newSpeed) 
    {
        moveSpeed = newSpeed;       
    }

    public virtual bool CanJump()
    {
        return groundChecker.isGrounded; 
    }
}
