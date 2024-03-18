using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : HostLocomotion
{
    public GroundChecker groundChecker = null;
    private float g = 1.0f, velocityY = 1.0f, gravityScale = 0f;
    private float jumpOffset = 0.5f;
    private bool isJumping = false;
    private bool hasFreeMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;

        rb2D = GetComponentInChildren<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2) * (jumpDistance / 2));
        gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2);

        if (hasFreeMovement)
        {
            rb2D.gravityScale = 0.0f;
        }
        else
        {
            rb2D.gravityScale = gravityScale;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isJumping)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, velocityY);
            isJumping = false;
        }
    }

    public override void Jump(float deltaX = 0)
    {
        if (!groundChecker.isGrounded || hasFreeMovement) return;
        isJumping = true;
        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
    }

    public override void Move(float deltaX, float deltaY)
    {
        if (hasFreeMovement)
        {
            FreeMove(deltaX, deltaY);
        }
        else
        {
            rb2D.velocity = new Vector2(deltaX * moveSpeed, rb2D.velocity.y);
        }
    }

    private void FreeMove(float deltaX, float deltaY)
    {
        rb2D.velocity = new Vector2(deltaX * moveSpeed, deltaY * moveSpeed);
    }

    public void EnableFreeMovement()
    {
        hasFreeMovement = true;
        rb2D.gravityScale = 0.0f;
    }

    public void DisableFreeMovement()
    {
        hasFreeMovement = false;
        rb2D.gravityScale = gravityScale;
    }

    public override void Attack()
    {
        return;
    }

    public override bool IsAttackReady()
    {
        return true;
    }
}
