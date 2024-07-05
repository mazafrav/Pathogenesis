using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : HostLocomotion
{
    private float gravityScale = 0f;

    private bool isJumping = false;
    private float heightJumped = 0f;
    private float originalY = 0f;

    private float originalMoveSpeed;

    [SerializeField]
    public Animator animator;
    [SerializeField]
    private GameObject playerBody;

    [Header("SFX")]
    [SerializeField]
    private AudioClip movementLoopClip;
    [SerializeField]
    private AudioClip FMAMoveLoopClip;
    [SerializeField]
    private AudioClip jumpClip;
    [SerializeField]
    private AudioClip landClip;  

    [SerializeField] private float distanceToFallToPlayLandClip = 2f;

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
        rb2D.gravityScale = g / Physics2D.gravity.y;

        originalMoveSpeed = moveSpeed;
        //audioSource.loop = true;
        GetAudioSource().clip = movementLoopClip;
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

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb2D.velocity.x) + Mathf.Abs(rb2D.velocity.y));
        if (groundChecker.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            originalY = playerBody.transform.position.y;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;

            if (rb2D.gravityScale > 0.0f)
            {
                float yDiff = Mathf.Abs(playerBody.transform.position.y - originalY);
                heightJumped = Mathf.Max(heightJumped, yDiff);
            }
        }

        if (heightJumped >= distanceToFallToPlayLandClip && groundChecker.isGrounded)
        {
            heightJumped = 0f;
            GetOneShotSource().PlayOneShot(landClip);
        }

        if (rb2D.velocity.x != 0)
        {
            if (!GetAudioSource().isPlaying)
            {
                GetAudioSource().Play();
            }
        }
        else if (rb2D.gravityScale <= 0.0f && rb2D.velocity.y != 0)
        {
            if (!GetAudioSource().isPlaying)
            {
                GetAudioSource().Play();
            }
        }
        else
        {
            GetAudioSource().Stop();
        }
        

        if (GameManager.Instance.isPaused)
        {
            GetAudioSource().Stop();
            GetOneShotSource().Stop();
        }
    }

    public override void Jump(float deltaX = 0)
    {
        if(rb2D)
        {
            if (coyoteTimeCounter <= 0f || rb2D.gravityScale <= 0.0f) return;
            isJumping = true;
            g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
            rb2D.gravityScale = g / Physics2D.gravity.y;
            animator.SetBool("Grounded", false);
            animator.SetBool("Jumping", true);
            heightJumped = 0f;

            GetOneShotSource().PlayOneShot(jumpClip);
        }
    }

    public override void Move(float deltaX, float deltaY)
    {
        if(!rb2D)
        {
            return;
        }

        if (rb2D.gravityScale <= 0.0f)
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

    public void EnableFreeMovement(float speedModifier = 1.0f)
    {
        GetAudioSource().clip = FMAMoveLoopClip;
        heightJumped = 0f;
        moveSpeed *= speedModifier;
        rb2D.gravityScale = 0.0f;
        animator.SetBool("IsInFreeMovement", true);
        isJumping = false;
        animator.SetBool("Jumping", false);
    }

    public void DisableFreeMovement()
    {
        GetAudioSource().clip = movementLoopClip;
        moveSpeed = originalMoveSpeed;
        rb2D.gravityScale = gravityScale;
        animator.SetBool("IsInFreeMovement", false);
    }

    public override void Attack(Vector3 target = default)
    {
        return;
    }

    public override bool IsAttackReady()
    {
        return true;
    }

    public override void ResetAttack()
    {
        return;
    }

    public override void Aim(Vector3 target = default)
    {
        return;
    }
}
