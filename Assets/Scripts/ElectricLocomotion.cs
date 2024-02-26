using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLocomotion : HostLocomotion
{
    [Header("Attack")]
    [SerializeField]
    private GameObject electricShockGameObject = null;
    [SerializeField]
    float rayRange = 1.0f;
    [SerializeField]
    private float cooldown = 1.0f;
    [SerializeField]
    public float ElectricShockRange { get; private set; } = 3.0f;
    [SerializeField]
    private float windUp = 0.5f;
    private float currentWindUpTime = 0.5f;
    [SerializeField]
    private float shockDuration = 0.5f;
    private float currentShockDuration = 0.5f;
    private Rigidbody2D rb2D = null;
    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    private float currentCooldownTime = 0.0f;


    void Start()
    {
        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;
        rb2D = GetComponentInParent<Rigidbody2D>();

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        electricShockGameObject.SetActive(false);
        electricShockGameObject.transform.localScale = new Vector3(ElectricShockRange * 2.0f, ElectricShockRange * 2.0f, electricShockGameObject.transform.localScale.z);
    }

    void Update()
    {
        if (currentCooldownTime > 0f)
        {
            currentCooldownTime = Mathf.Max(currentCooldownTime + Time.deltaTime, 0f);
        }

        if (currentWindUpTime > 0f)
        {
            currentWindUpTime = Mathf.Max(currentWindUpTime + Time.deltaTime, 0f);
            if (currentWindUpTime == 0f)
            {
                ActivateShock();
            }
        }

        if (currentShockDuration > 0f)
        {
            currentShockDuration = Mathf.Max(currentShockDuration + Time.deltaTime, 0f);
            if (currentShockDuration == 0f)
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
        rb2D.velocity = new Vector2(Mathf.Sign(deltaX) * moveSpeed, rb2D.velocity.y);
    }

    public override void Attack()
    {
        currentWindUpTime = windUp;
    }

    private void ActivateShock()
    {
        electricShockGameObject.SetActive(true);
    }

    private void DeactivateShock()
    {
        electricShockGameObject.SetActive(false);
    }

    public override bool IsAttackReady()
    {
        return currentCooldownTime == 0f && currentWindUpTime == 0f;
    }

    public bool IsWindingUp()
    {
        return currentWindUpTime > 0f;
    }

    private void ResetAttack()
    {
        currentCooldownTime = 0f;
        currentWindUpTime = 0f;
        currentShockDuration = 0f;
    }
}


