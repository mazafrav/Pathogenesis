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

    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    private float currentWindUpTime = 0.5f;
    private float currentCooldownTime = 0.0f;
    private float currentShockDuration = 0.0f;
    private bool isSeeingPlayer = false;

    public float ElectricShockRange { get { return electricShockRange; } }
    public bool IsSeeingPlayer { set { isSeeingPlayer = value; } }

    void Start()
    {
        currentWindUpTime = windUp;
        currentCooldownTime = cooldown;
        currentShockDuration = shockDuration;
        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;
        rb2D = GetComponentInParent<Rigidbody2D>();

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        electricShockGameObject.SetActive(false);
        electricShockGameObject.transform.localScale = new Vector3(electricShockRange * 2.0f, electricShockRange * 2.0f, electricShockGameObject.transform.localScale.z);
    }

    void Update()
    {
        if (isSeeingPlayer)
        {
            //Debug.Log("Cooldown: " + currentCooldownTime);
            if (currentCooldownTime <= 0f)
            {

                currentWindUpTime = Mathf.Max(currentWindUpTime - Time.deltaTime, 0f);
                //Debug.Log("Wind up: " + currentWindUpTime);
                if (currentWindUpTime <= 0f)
                {
                    ActivateShock();
                    
                }           
            }
            else if (!electricShockGameObject.activeSelf && currentCooldownTime > 0f)
            {
                currentCooldownTime = Mathf.Max(currentCooldownTime - Time.deltaTime, 0f);
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
        rb2D.velocity = new Vector2(moveSpeed * deltaX, velocityY);
    }

    public override void Move(float deltaX, float deltaY = 0f)
    {
        rb2D.velocity = new Vector2(deltaX * moveSpeed, rb2D.velocity.y);
    }

    public override void Attack()
    {
        if(!IsAttackReady()) return;
        currentWindUpTime = windUp;       
    }

    private void ActivateShock()
    {
        electricShockGameObject.SetActive(true);
        //currentWindUpTime = windUp;
        currentCooldownTime = cooldown;
    }

    private void DeactivateShock()
    {
        electricShockGameObject.SetActive(false);
        currentShockDuration = shockDuration;
        currentCooldownTime = cooldown;
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
        currentCooldownTime = cooldown;
        currentWindUpTime = windUp;
    }
}


