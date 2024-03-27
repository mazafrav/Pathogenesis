using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLocomotion : HostLocomotion
{
    [Header("Attack")]
    [SerializeField]
    private GameObject electricShockGameObject = null;
    [SerializeField]
    private float electricShockRange = 4.0f;
    [SerializeField]
    private float cooldown = 1.0f;
    [SerializeField]
    private float windUp = 0.5f;
    [SerializeField]
    private float shockDuration = 0.5f;

    [Header("Feedback")]
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    [SerializeField]
    private Color colorWhileWindUp, colorWhileCooldown;

    private Color colorWhileMoving;

    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    private float currentWindUpTime = 0.0f, currentCooldownTime = 0.0f, currentShockDuration = 0.0f;

    private PlayerController playerController;
    private GroundChecker groundChecker;
    public float ElectricShockRange { get { return electricShockRange; } }


    void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        groundChecker = GetComponentInChildren<GroundChecker>();

        colorWhileMoving = spriteRenderers[0].color;

        currentShockDuration = shockDuration;
        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;
        rb2D = GetComponent<Rigidbody2D>();

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        electricShockGameObject.SetActive(false);
        electricShockGameObject.transform.localScale = new Vector3(electricShockRange * 2.0f, electricShockRange * 2.0f, electricShockGameObject.transform.localScale.z);
    }

    void Update()
    {      
        //Debug.Log("Cooldown: " + currentCooldownTime);
        if (currentCooldownTime <= 0f && currentWindUpTime > 0f)
        {
            currentWindUpTime = Mathf.Max(currentWindUpTime - Time.deltaTime, 0f);
            //Debug.Log("Wind up: " + currentWindUpTime);
            if (currentWindUpTime <= 0f)
            {
                ActivateShock();                   
            }
            //We don't move player who is controlling the electric enemy when is winding up
            //else if (playerController && playerController.locomotion.GetType() == this.GetType())
            //{
            //    playerController.DeltaX = 0.0f; 
            //}
        }
        else if (!electricShockGameObject.activeSelf && currentCooldownTime > 0f)
        {
            currentCooldownTime = Mathf.Max(currentCooldownTime - Time.deltaTime, 0f);

            //We only apply cooldown feedback when the player is controlling the electric enemy
            if(playerController && playerController.locomotion.GetType() == this.GetType())
            {
                if(currentCooldownTime <= 0f) 
                {
                    ChangeSpritesColor(colorWhileMoving);
                }
                else
                {
                    ChangeSpritesColor(colorWhileCooldown);
                }
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

    public override void Attack()
    {
        if(!IsAttackReady()) return;
        ChangeSpritesColor(colorWhileWindUp);
        currentWindUpTime = windUp;
    }

    private void ActivateShock()
    {
        electricShockGameObject.SetActive(true);
        currentCooldownTime = cooldown;
        ChangeSpritesColor(colorWhileMoving);
    }

    private void DeactivateShock()
    {
        electricShockGameObject.SetActive(false);
        currentShockDuration = shockDuration;
    }

    public override bool IsAttackReady()
    {
        return currentCooldownTime <= 0f && currentWindUpTime <= 0f;
    }

    public bool IsWindingUp()
    {
        return currentWindUpTime > 0f;
    }

    public bool IsCooldownFinished() 
    {  
        return currentCooldownTime <= 0.0f;  
    }

    public void ResetAttack()
    {
        currentCooldownTime = 0f;
        currentWindUpTime = 0f;
        ChangeSpritesColor(colorWhileMoving);
    }

    private void ChangeSpritesColor(Color newColor)
    {
        spriteRenderers[0].color = newColor;
        spriteRenderers[1].color = newColor;
    }
}


