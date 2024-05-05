using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallineLocomotion : HostLocomotion
{
    [Header("Attack")]
    [SerializeField]
    private GameObject graphics;
    [SerializeField]
    private float maxAngularSpeed = 10f;

    [SerializeField]
    private CrystallineStab crystallineStab;
    [SerializeField]
    private float stabRange = 4.0f;
    [SerializeField]
    private float cooldown = 1.0f;
    [SerializeField]
    private float windUp = 0.5f;
    [SerializeField]
    private float stabDuration = 0.5f;

    [Header("Feedback")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Color colorWhileWindUp, colorWhileCooldown;

    private Color defaultColor;

    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    private float currentWindUpTime = 0.0f, currentCooldownTime = 0.0f, currentStabDuration = 0.0f;

    private PlayerController playerController;
    private GroundChecker groundChecker;
    private HostAbsorption absorption;

    void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        groundChecker = GetComponentInChildren<GroundChecker>();
        absorption = GetComponent<HostAbsorption>();

        defaultColor = spriteRenderer.color;

        currentStabDuration = stabDuration;
        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;
        rb2D = GetComponent<Rigidbody2D>();

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);
    }

    void Update()
    {
        if (currentCooldownTime <= 0f && currentWindUpTime > 0f)
        {
            currentWindUpTime = Mathf.Max(currentWindUpTime - Time.deltaTime, 0f);

            ChangeSpritesColor(Color.Lerp(GetCurrentColor(), colorWhileWindUp, 1.0f - currentWindUpTime));
            if (currentWindUpTime <= 0f)
            {
                ActivateStab();
            }
        }
        else if (crystallineStab.isDamageActive)
        {
            currentStabDuration = Mathf.Max(currentStabDuration - Time.deltaTime, 0f);
            crystallineStab.MoveStab(1 - Mathf.Abs(1 - 2 * (currentStabDuration / stabDuration)));
            if (currentStabDuration <= 0f)
            {
                DeactivateStab();
            }
        }
        else if (!crystallineStab.isDamageActive && currentCooldownTime > 0f)
        {
            currentCooldownTime = Mathf.Max(currentCooldownTime - Time.deltaTime, 0f);
            //We only apply cooldown feedback when the player is controlling the electric enemy
            if (playerController && playerController.locomotion.GetType() == this.GetType())
            {
                ChangeSpritesColor(Color.Lerp(colorWhileCooldown, GetCurrentColor(), 1.0f - currentCooldownTime));
            }
        }

    }

    public override void Jump(float deltaX)
    {
        if (currentWindUpTime > 0f) return;
        if (groundChecker.isGrounded)
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
        if (IsAttackReady())
        {
            currentWindUpTime = windUp;
        }
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }


    private void ActivateStab()
    {
        crystallineStab.isDamageActive = true;
    }

    private void DeactivateStab()
    {
        currentStabDuration = stabDuration;
        currentCooldownTime = cooldown;
        currentWindUpTime = 0f;
        Color currentColor = GetCurrentColor();
        ChangeSpritesColor(currentColor);
        crystallineStab.isDamageActive = false;
        crystallineStab.MoveStab(0f);
    }

    public override bool IsAttackReady()
    {
        return currentCooldownTime <= 0f && currentWindUpTime <= 0f && !crystallineStab.isDamageActive;
    }

    public override void ResetAttack()
    {
        currentCooldownTime = 0f;
        currentWindUpTime = 0f;
        Color currentColor = GetCurrentColor();
        ChangeSpritesColor(currentColor);
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
        float angle = AngleBetweenPoints(target, transform.position);
        if(angle < 0f)
        {
            if(angle > -90f)
            {
                angle = 0f;
            }
            else
            {
                angle = 180f;
            }
        }
        graphics.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public override void SetPossessingParameters()
    {
        base.SetPossessingParameters();

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        CrystallineEnemyPossessingParameters crystallinePossessingParameters = (CrystallineEnemyPossessingParameters)possessingParameters;
        stabRange = crystallinePossessingParameters.stabRange;
        cooldown = crystallinePossessingParameters.cooldown;
        windUp = crystallinePossessingParameters.windUp;
        stabDuration = crystallinePossessingParameters.stabDuration;
    }
}
