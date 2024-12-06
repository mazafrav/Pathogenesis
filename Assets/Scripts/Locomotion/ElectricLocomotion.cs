using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricLocomotion : HostLocomotion
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Attack")]
    [SerializeField]
    private GameObject shockGameObject = null;
    [SerializeField]
    private GameObject shockBaseObject = null;
    [SerializeField]
    private GameObject followGameObject = null;
    [SerializeField]
    private GameObject attackGameObject = null;
    [SerializeField]
    private float followRange = 4.0f;
    [SerializeField]
    private float attackRange = 2.5f;
    //[SerializeField]
    //private float shockRange = 1.5f;
    [SerializeField]
    private float cooldown = 1.0f;
    [SerializeField]
    private float windUp = 0.5f;
    [SerializeField]
    private float shockRemainingTime = 0.3f;
    [SerializeField]
    private ParticleSystem electricShockVFX;
    //[SerializeField]
    //private float shockDuration = 1.0f;

    [Header("Feedback")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Color colorWhileWindUp, colorWhileCooldown;
    [SerializeField]
    private AudioClip electricShockClip;

    [Header("Movement")]
    [SerializeField]
    private float deltaXModifier = 0.5f;
    //[SerializeField]
    //private float deltaYModifier = 1.2f;
    [SerializeField]
    private float speedModifier = 1.0f;

    [Header("Propulsion Jump")]
    [SerializeField] private float propulsionHeight = 2.0f;
    [SerializeField] private float propulsionTime = 1.0f;
    [SerializeField] private float planningGravity = 0.1f;
    [SerializeField] private ParticleSystem jumpParticles;

    [Header("Lights")]
    [SerializeField] GameObject ligthSource;
    [SerializeField] GameObject possessedLightSource;

    private float currentPlanningGravity = 0.1f;
    //private float newYPosition = 0.0f;
    private Vector3 startPosition = Vector3.zero;
    private float propulsingTimeCounter = 0.0f;
    private bool isPropulsing = false, isPlanning = false;

    private float originalMoveSpeed;

    private Color defaultColor;

    private float currentWindUpTime = 0.0f, currentCooldownTime = 0.0f;/*, currentShockDuration = 0.0f;*/

    public float currentRemainingShockTime { get; set; } = 0.0f;
    public bool inAttackRange { get; set; } = false;

    private HostAbsorption absorption;
    private bool hasAttacked = false;
    public float FollowRange { get { return followRange; } }
    public float AttackRange { get { return attackRange; } }

    public float ShockRemainingTime() => shockRemainingTime;

    void Start()
    {
        velocityY = propulsionHeight / propulsionTime;

        originalMoveSpeed = moveSpeed;
        playerController = GameManager.Instance.GetPlayerController();

        absorption = GetComponent<HostAbsorption>();

        defaultColor = spriteRenderer.color;

        //currentShockDuration = shockDuration;
        //jumpDistance += jumpOffset;
        //jumpHeight += jumpOffset;
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 1.0f;

        //g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        //rb2D.gravityScale = g / Physics2D.gravity.y;
        //velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        shockGameObject.SetActive(false);
        shockBaseObject.SetActive(false);

        followGameObject.transform.localScale = new Vector3(followRange, followRange, followGameObject.transform.localScale.z);
        //shockGameObject.transform.localScale = new Vector3(0.25f, 2*shockRange, shockGameObject.transform.localScale.z);
        attackGameObject.transform.localScale = new Vector3(attackRange, attackRange, attackGameObject.transform.localScale.z);
    }

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb2D.velocity.x) + Mathf.Abs(rb2D.velocity.y));

        //Debug.Log("Cooldown: " + currentCooldownTime);
        if (currentCooldownTime <= 0f && currentWindUpTime > 0f)
        {
            currentWindUpTime = Mathf.Max(currentWindUpTime - Time.deltaTime, 0f);

            ChangeSpritesColor(Color.Lerp(GetCurrentColor(), colorWhileWindUp, 1.0f - currentWindUpTime));
            //Debug.Log("Wind up: " + currentWindUpTime);
            if (currentWindUpTime <= 0f)
            {
                ActivateShock();                   
                Color currentColor = GetCurrentColor();
                ChangeSpritesColor(currentColor);
            }           
        }
        else if (!shockGameObject.activeSelf && currentCooldownTime > 0f)
        {
            currentCooldownTime = Mathf.Max(currentCooldownTime - Time.deltaTime, 0f);

            //We only apply cooldown feedback when the player is controlling the electric enemy
            if(playerController && playerController.locomotion.GetType() == this.GetType())
            {
                ChangeSpritesColor(Color.Lerp(colorWhileCooldown, GetCurrentColor(), 1.0f - currentCooldownTime));
            }
        }

        //Only when we are possessed the shock is deactivated after x time
        //if (transform.parent!=null && shockGameObject.activeSelf)
        //{
        //    currentShockDuration = Mathf.Max(currentShockDuration - Time.deltaTime, 0f);
        //    if (currentShockDuration <= 0f)
        //    {
        //        DeactivateShock();
        //    }
        //}

        //Calculating new enemy position while jumping
        if (isPropulsing)
        {
            propulsingTimeCounter += Time.deltaTime;
            if (!jumpParticles.isPlaying)
            {
                jumpParticles.Play();
            }
            if (transform.position.y > startPosition.y + propulsionHeight || propulsingTimeCounter >= propulsionTime) 
            //Has reached the requiered height or time
            {
                animator.Play("ElecEnemyFloatAnim");
                isPropulsing = false;
                jumpParticles.Stop();
                isPlanning = true;
                propulsingTimeCounter = 0f;

                //jumpEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
            else
            {
                //newYPosition += velocityY * Time.deltaTime;
            }

        }

        //Change gravity while planning
        if (!isPropulsing && isPlanning && rb2D.velocity.y < 0f)
        {
            currentPlanningGravity = planningGravity + Mathf.Abs(playerController.GetDeltaY());
            Mathf.Clamp(currentPlanningGravity, planningGravity, 1.0f);
            
            rb2D.gravityScale = currentPlanningGravity; 
        }
        else if(groundChecker.isGrounded) //Reached the floor, he is not planning
        {
            if (isPlanning)
            {
                animator.Play("ElecEnemyLandAnim");
                jumpEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
            isPlanning = false;
            rb2D.gravityScale = 1.0f;
        }
    }

    private void FixedUpdate()
    {
        //Propulsing the enemy
        if (isPropulsing)
        {
            if (transform.position.y < startPosition.y + propulsionHeight)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x , velocityY);//newYPosition);
            }
        }
    }

    public override void Jump(float deltaX)
    {
        if (currentWindUpTime > 0f) return;
        //if(groundChecker.isGrounded)
        //{
        //    rb2D.velocity = new Vector2(moveSpeed * deltaX, velocityY);
        //}

        if (groundChecker.isGrounded)
        {
            rb2D.gravityScale = 1f;
            startPosition = transform.position;
            //newYPosition = 0;
            propulsingTimeCounter = 0f;
            isPropulsing = true;
            animator.Play("ElecEnemyJumpAnim");

            jumpEventInstance.start();
        }
       
    }
    public override void JumpCancel() 
    {
        /*
        isPlanning = true;
        isPropulsing = false;
        jumpParticles.Stop();
        */
    }

    public override void Move(float deltaX, float deltaY = 0f)
    {      
        //if (groundChecker.isGrounded && IsWindingUp() && IsCooldownFinished()) //while charging his attack dont move
        //{
        //    rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
        //}
        //else
        //{       
        //when is jumping and possessed we apply a modifier to the X
        if (!groundChecker.isGrounded && transform.parent != null)
        {           
            rb2D.velocity = new Vector2(deltaXModifier * deltaX * moveSpeed, rb2D.velocity.y);          
        }
        else //normal movement
        {
            rb2D.velocity = new Vector2(deltaX * moveSpeed, rb2D.velocity.y);
        }

        //}
    }

    public override void Attack(Vector3 target = default)
    {
        if(!IsAttackReady() || shockGameObject.activeSelf) return;
        currentWindUpTime = windUp;
        //hasAttacked = true;
        //currentShockDuration = shockDuration;
    }

    private void ActivateShock()
    {
        currentRemainingShockTime = shockRemainingTime;
        inAttackRange = true;
        hasAttacked = true;
        shockGameObject.SetActive(true);
        shockBaseObject.SetActive(true);
        moveSpeed *= speedModifier;
        electricShockVFX.Play();
    }
    public override void DeactivateAttack()
    {
        base.DeactivateAttack();
        DeactivateShock();
    }
    public void DeactivateShock()
    {
        shockGameObject.SetActive(false);
        shockBaseObject.SetActive(false);
        electricShockVFX.Stop();

        if (hasAttacked)
        {
            //currentShockDuration = shockDuration;
            currentCooldownTime = cooldown;
            currentWindUpTime = 0f;
            hasAttacked = false;
            moveSpeed = originalMoveSpeed;
            Color currentColor = GetCurrentColor();
            ChangeSpritesColor(currentColor);
        }

        //if(transform.parent != null) //Is possessed
        //{
        //    currentShockDuration = shockDuration;
        //    currentCooldownTime = cooldown;
        //    currentWindUpTime = 0f;
        //    shockGameObject.SetActive(false);
        //}
        //else
        //{

        //    if (hasAttacked)
        //    {
        //        currentCooldownTime = cooldown;
        //        currentWindUpTime = 0f;
        //        hasAttacked = false;
        //    }          
        //        shockGameObject.SetActive(false);
        //}


    }

    public override bool IsAttackReady()
    {
        return currentCooldownTime <= 0f && currentWindUpTime <= 0f;
    }

    public override void ResetAttack()
    {
        currentCooldownTime = 0f;
        currentWindUpTime = 0f;
        ChangeSpritesColor(GetCurrentColor());
        shockGameObject.SetActive(false); 
        shockBaseObject.SetActive(false);
        attackGameObject.SetActive(false); 
        followGameObject.SetActive(false);
    }

    public bool IsWindingUp()
    {
        return currentWindUpTime > 0f;
    }

    public bool IsCooldownFinished() 
    {  
        return currentCooldownTime <= 0.0f;  
    }

    private Color GetCurrentColor() { return transform.parent != null ? absorption.possessingColor : defaultColor; }

    private void ChangeSpritesColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }

    public override void Aim(Vector3 target = default)
    {
        return;
    }

    public override void SetPossessingParameters()
    {
        base.SetPossessingParameters();
        originalMoveSpeed = moveSpeed;

        ElectricEnemyPossessingParameters electricPossessingParameters = (ElectricEnemyPossessingParameters)possessingParameters;

        //g =( (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f)) ) * electricPossessingParameters.gravityModifier;
        //rb2D.gravityScale = g / Physics2D.gravity.y;
        //velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        cooldown = electricPossessingParameters.cooldown;
        windUp = electricPossessingParameters.windUp;
        shockRemainingTime = electricPossessingParameters.shockRemainingTime;
        //shockDuration = electricPossessingParameters.shockDuration;
        //followRange = electricPossessingParameters.electricFollowRange;
        //followGameObject.transform.localScale = new Vector3(followRange, followRange, shockGameObject.transform.localScale.z);

        //GameManager.Instance.SetMusicSelectionIndex(4);

        //change light source
        ligthSource.SetActive(false);
        possessedLightSource.SetActive(true);
    }

    public override void CancelAttack()
    {
        
    }

    public void StopJumpLoopSFX()
    {
        jumpEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        jumpEventInstance.release();
    }
}


