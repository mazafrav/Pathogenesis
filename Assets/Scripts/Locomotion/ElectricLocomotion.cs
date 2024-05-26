using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLocomotion : HostLocomotion
{
    [Header("Attack")]
    [SerializeField]
    private GameObject electricShockGameObject = null;
    [SerializeField]
    private GameObject electricShockRangeGameObject = null;
    [SerializeField]
    private float electricShockRange = 4.0f;
    [SerializeField]
    private float cooldown = 1.0f;
    [SerializeField]
    private float windUp = 0.5f;
    [SerializeField]
    private float shockDuration = 1.0f;

    [Header("Feedback")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Color colorWhileWindUp, colorWhileCooldown;

    private Color defaultColor;

    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    private float currentWindUpTime = 0.0f, currentCooldownTime = 0.0f, currentShockDuration = 0.0f;

    private PlayerController playerController;
    private GroundChecker groundChecker;
    private HostAbsorption absorption;
    public float ElectricShockRange { get { return electricShockRange; } }



    void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        groundChecker = GetComponentInChildren<GroundChecker>();
        absorption = GetComponent<HostAbsorption>();

        defaultColor = spriteRenderer.color;

        currentShockDuration = shockDuration;
        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;
        rb2D = GetComponent<Rigidbody2D>();

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        electricShockGameObject.SetActive(false);

        electricShockRangeGameObject.transform.localScale = new Vector3(electricShockRange, electricShockRange, electricShockGameObject.transform.localScale.z);
    }

    void Update()
    {      
        //Debug.Log("Cooldown: " + currentCooldownTime);
        if (currentCooldownTime <= 0f && currentWindUpTime > 0f)
        {
            currentWindUpTime = Mathf.Max(currentWindUpTime - Time.deltaTime, 0f);

            ChangeSpritesColor(Color.Lerp(GetCurrentColor(), colorWhileWindUp, 1.0f - currentWindUpTime));
            //Debug.Log("Wind up: " + currentWindUpTime);
            if (currentWindUpTime <= 0f)
            {
                ActivateShock();                   
            }           
        }
        else if (!electricShockGameObject.activeSelf && currentCooldownTime > 0f)
        {
            currentCooldownTime = Mathf.Max(currentCooldownTime - Time.deltaTime, 0f);

            //We only apply cooldown feedback when the player is controlling the electric enemy
            if(playerController && playerController.locomotion.GetType() == this.GetType())
            {
                ChangeSpritesColor(Color.Lerp(colorWhileCooldown, GetCurrentColor(), 1.0f - currentCooldownTime));
            }
        }

                 

        if (electricShockGameObject.activeSelf)
        {
            currentShockDuration = Mathf.Max(currentShockDuration - Time.deltaTime, 0f);
            if (currentShockDuration <= 0f)
            {
                DeactivateShock();
            }
        }
    }

    public override void Jump(float deltaX)
    {
        if (currentWindUpTime > 0f) return;
        if(groundChecker.isGrounded)
        {
            rb2D.velocity = new Vector2(moveSpeed * deltaX, velocityY);
        }
    }

    public override void Move(float deltaX, float deltaY = 0f)
    {
        if (IsWindingUp() && IsCooldownFinished())
        {
            rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(deltaX * moveSpeed, rb2D.velocity.y);
        }
    }

    public override void Attack(Vector3 target = default)
    {
        if(!IsAttackReady()) return;
        currentWindUpTime = windUp;
    }

    private void ActivateShock()
    {
        electricShockGameObject.SetActive(true);
    }

    private void DeactivateShock()
    {
        electricShockGameObject.SetActive(false);
        currentShockDuration = shockDuration;
        currentCooldownTime = cooldown;
        currentWindUpTime = 0f;
        Color currentColor = GetCurrentColor();
        ChangeSpritesColor(currentColor);
    }

    public override bool IsAttackReady()
    {
        return currentCooldownTime <= 0f && currentWindUpTime <= 0f;
    }

    public override void ResetAttack()
    {
        //currentCooldownTime = 0f;
        //currentWindUpTime = 0f;
        //Color currentColor = GetCurrentColor();
        //ChangeSpritesColor(currentColor);
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

        ElectricEnemyPossessingParameters electricPossessingParameters = (ElectricEnemyPossessingParameters)possessingParameters;

        g =( (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f)) ) * electricPossessingParameters.gravityModifier;
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        cooldown = electricPossessingParameters.cooldown;
        windUp = electricPossessingParameters.windUp;
        shockDuration = electricPossessingParameters.shockDuration;
        electricShockRange = electricPossessingParameters.electricShockRange;
        electricShockRangeGameObject.transform.localScale = new Vector3(electricShockRange, electricShockRange, electricShockGameObject.transform.localScale.z);
    }

    public float GetShockDuration()
    {
        return shockDuration;
    }
}


