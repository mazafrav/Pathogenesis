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
    [SerializeField]
    private float cooldown = 1.0f;
    [SerializeField]
    private float windUp = 0.5f;
    [SerializeField]
    private float shockRemainingTime = 0.3f;
    [SerializeField]
    private ParticleSystem electricShockVFX;

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
    private Vector3 startPosition = Vector3.zero;
    private float propulsingTimeCounter = 0.0f;
    private bool isPropulsing = false, isPlanning = false;

    private float originalMoveSpeed;

    private Color defaultColor;

    private float currentWindUpTime = 0.0f, currentCooldownTime = 0.0f;

    private HostAbsorption absorption;
    private ElectricEnemy electricEnemy;
    private bool hasAttacked = false;
    private bool isAttacking = false;

    public float CurrentRemainingShockTime { get; set; } = 0.0f;
    public bool InAttackRange { get; set; } = false;
    public float FollowRange { get { return followRange; } }
    public float AttackRange { get { return attackRange; } }

    public float ShockRemainingTime() => shockRemainingTime;

    void Start()
    {
        velocityY = propulsionHeight / propulsionTime;

        originalMoveSpeed = moveSpeed;
        playerController = GameManager.Instance.GetPlayerController();

        electricEnemy = GetComponent<ElectricEnemy>();
        absorption = GetComponent<HostAbsorption>();

        defaultColor = spriteRenderer.color;

        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 1.0f;

        shockGameObject.SetActive(false);
        shockBaseObject.SetActive(false);

        followGameObject.transform.localScale = new Vector3(followRange, followRange, followGameObject.transform.localScale.z);
        attackGameObject.transform.localScale = new Vector3(attackRange, attackRange, attackGameObject.transform.localScale.z);
    }

    void Update()
    {       
        //Attacking when is possessed
        if (electricEnemy.IsPossesed)
        {
            if (!isAttacking && InAttackRange && CurrentRemainingShockTime > 0.0f)
            {
                //We wait a bit to deactivate the shock
                CurrentRemainingShockTime -= Time.deltaTime;               

                if (CurrentRemainingShockTime <= 0.0f)
                {
                    DeactivateShock();
                    InAttackRange = false;
                }
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(rb2D.velocity.x) + Mathf.Abs(rb2D.velocity.y));

        if (currentCooldownTime <= 0f && currentWindUpTime > 0f)
        {
            currentWindUpTime = Mathf.Max(currentWindUpTime - Time.deltaTime, 0f);

            ChangeSpritesColor(Color.Lerp(GetCurrentColor(), colorWhileWindUp, 1.0f - currentWindUpTime));

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

        //Calculating new enemy position while jumping
        if (isPropulsing)
        {
            propulsingTimeCounter += Time.deltaTime;
            if (!jumpParticles.isPlaying)
            {
                jumpParticles.Play();
            }

            //Has reached the requiered height or time
            if (transform.position.y > startPosition.y + propulsionHeight || propulsingTimeCounter >= propulsionTime) 
            {
                animator.Play("ElecEnemyFloatAnim");
                isPropulsing = false;
                jumpParticles.Stop();
                isPlanning = true;
                propulsingTimeCounter = 0f;
            }
            //else
            //{
            //    newYPosition += velocityY * Time.deltaTime;
            //}

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
                rb2D.velocity = new Vector2(rb2D.velocity.x , velocityY);
            }
        }
    }

    public override void Jump(float deltaX)
    {
        if (currentWindUpTime > 0f) return;

        if (groundChecker.isGrounded)
        {
            rb2D.gravityScale = 1f;
            startPosition = transform.position;
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
        if (!groundChecker.isGrounded && electricEnemy.IsPossesed)
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
        isAttacking = true;
       
    }

    public override void CancelAttack()
    {
        isAttacking = false;
    }

    private void ActivateShock()
    {
        CurrentRemainingShockTime = shockRemainingTime;
        InAttackRange = true;
        hasAttacked = true;
        shockGameObject.SetActive(true);
        shockBaseObject.SetActive(true);
        moveSpeed *= speedModifier;
        electricShockVFX.Play();
    }

    public void DeactivateShock()
    {
        shockGameObject.SetActive(false);
        shockBaseObject.SetActive(false);
        electricShockVFX.Stop();

        if (hasAttacked)
        {
            currentCooldownTime = cooldown;
            currentWindUpTime = 0f;
            hasAttacked = false;
            moveSpeed = originalMoveSpeed;
            Color currentColor = GetCurrentColor();
            ChangeSpritesColor(currentColor);
        }
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

        cooldown = electricPossessingParameters.cooldown;
        windUp = electricPossessingParameters.windUp;
        shockRemainingTime = electricPossessingParameters.shockRemainingTime;

        //change light source
        ligthSource.SetActive(false);
        possessedLightSource.SetActive(true);
    }

    public void StopJumpLoopSFX()
    {
        jumpEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        jumpEventInstance.release();
    }
}


