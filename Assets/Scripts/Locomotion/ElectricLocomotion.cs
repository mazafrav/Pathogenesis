using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLocomotion : HostLocomotion
{
    [Header("Attack")]
    [SerializeField]
    private GameObject shockGameObject = null;
    [SerializeField]
    private GameObject followGameObject = null;
    [SerializeField]
    private GameObject attackGameObject = null;
    [SerializeField]
    private float followRange = 4.0f;
    [SerializeField]
    private float attackRange = 2.5f;
    [SerializeField]
    private float shockRange = 1.5f;
    [SerializeField]
    private float cooldown = 1.0f;
    [SerializeField]
    private float windUp = 0.5f;
    //[SerializeField]
    //private float shockDuration = 1.0f;

    [Header("Feedback")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Color colorWhileWindUp, colorWhileCooldown;

    [Header("Movement")]
    [SerializeField]
    private float deltaXModifier = 0.5f;
    [SerializeField]
    private float deltaYModifier = 1.2f;
    [SerializeField]
    private float speedModifier = 1.0f;

    private float originalMoveSpeed;

    private Color defaultColor;

    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    private float currentWindUpTime = 0.0f, currentCooldownTime = 0.0f, currentShockDuration = 0.0f;

    private PlayerController playerController;

    private HostAbsorption absorption;
    private bool hasAttacked = false;
    public float FollowRange { get { return followRange; } }
    public float AttackRange { get { return attackRange; } }



    void Start()
    {
        originalMoveSpeed = moveSpeed;
        playerController = GameManager.Instance.GetPlayerController();

        absorption = GetComponent<HostAbsorption>();

        defaultColor = spriteRenderer.color;

        //currentShockDuration = shockDuration;
        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;
        rb2D = GetComponent<Rigidbody2D>();

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        shockGameObject.SetActive(false);

        followGameObject.transform.localScale = new Vector3(2*followRange, 2*followRange, followGameObject.transform.localScale.z);
        shockGameObject.transform.localScale = new Vector3(2*shockRange, 2*shockRange, shockGameObject.transform.localScale.z);
        attackGameObject.transform.localScale = new Vector3(2*attackRange, 2*attackRange, attackGameObject.transform.localScale.z);
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
        //if (groundChecker.isGrounded && IsWindingUp() && IsCooldownFinished()) //while charging his attack dont move
        //{
        //    rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
        //}
        //else
        //{
            //when is jumping and possessed we apply a modifier to the X and Y direction
            if (!groundChecker.isGrounded && transform.parent != null)
            {
                float Y = (/*rb2D.velocity.y > 0.0f &&*/ deltaY < 0)? deltaYModifier * deltaY : rb2D.velocity.y;
                rb2D.velocity = new Vector2(deltaXModifier * deltaX * moveSpeed, Y);
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
        hasAttacked = true;
        //currentShockDuration = shockDuration;
    }

    private void ActivateShock()
    {
        shockGameObject.SetActive(true);
        moveSpeed *= speedModifier;
    }
    public override void DeactivateAttack()
    {
        base.DeactivateAttack();
        DeactivateShock();
    }
    public void DeactivateShock()
    {
        shockGameObject.SetActive(false);
        if (hasAttacked)
        {
            //currentShockDuration = shockDuration;
            currentCooldownTime = cooldown;
            currentWindUpTime = 0f;
            hasAttacked = false;
            moveSpeed = originalMoveSpeed;
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

        Color currentColor = GetCurrentColor();
        ChangeSpritesColor(currentColor);
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
        attackGameObject.SetActive(false);  
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
        //shockDuration = electricPossessingParameters.shockDuration;
        followRange = electricPossessingParameters.electricShockRange;
        followGameObject.transform.localScale = new Vector3(followRange, followRange, shockGameObject.transform.localScale.z);

        GameManager.Instance.SetMusicSelectionIndex(4);
    }

    //public float GetShockDuration()
    //{
    //    return shockDuration;
    //}
}


