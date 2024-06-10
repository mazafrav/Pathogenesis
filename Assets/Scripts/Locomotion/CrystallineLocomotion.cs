using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField]
    private GroundChecker wallCheckerR;
    [SerializeField]
    private GroundChecker wallCheckerL;
    [SerializeField]
    private GroundChecker ceilChecker;
    private HostAbsorption absorption;
    private bool isClimbing = false;
    private float climbedDistance = 0f;
    private CrystalineEnemy enemyIA;
    private bool flipRot = true;


    void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        absorption = GetComponent<HostAbsorption>();

        enemyIA = GetComponent<CrystalineEnemy>();

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
        if (groundChecker.isGrounded)
        {
            climbedDistance = 0f;
        }

        if (isClimbing && !wallCheckerL.isGrounded && !wallCheckerR.isGrounded && !ceilChecker.isGrounded)
        {
            rb2D.gravityScale = g / Physics2D.gravity.y;
            isClimbing = false;
        }
        else if (!isClimbing && (wallCheckerL.isGrounded || wallCheckerR.isGrounded || ceilChecker.isGrounded))
        {
            isClimbing = true;
            rb2D.gravityScale = 0;
        }

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

    public override void Jump(float deltaY)
    {
        return;
        // if (IsWindingUp() && IsCooldownFinished())
        // {
        //     rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
        // }
        // else
        // {
        //     if (!isClimbing && (wallCheckerL.isGrounded || wallCheckerR.isGrounded))
        //     {
        //         isClimbing = true;
        //         rb2D.gravityScale = 0f;
        //     }
        //     if (isClimbing)
        //     {
        //         rb2D.velocity = new Vector2(rb2D.velocity.x, deltaY * moveSpeed);
        //     }
        // }
        // if (currentWindUpTime > 0f) return;
        // if (groundChecker.isGrounded)
        // {
        //     rb2D.velocity = new Vector2(moveSpeed * deltaX, velocityY);
        // }
    }

    public override void Move(float deltaX, float deltaY = 0f)
    {
        if (IsWindingUp() && IsCooldownFinished())
        {
            rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
        }
        else
        {
            if (isClimbing)
            {
                rb2D.velocity = new Vector2(deltaX * moveSpeed, deltaY * moveSpeed);
            }
            else
            {
                rb2D.velocity = new Vector2(deltaX * moveSpeed, rb2D.velocity.y);
            }
        }
    }

    public override void Attack(Vector3 target = default)
    {
        if (IsAttackReady())
        {
            currentWindUpTime = windUp;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
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
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
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
        if (crystallineStab.isDamageActive) return;
        if (target == default) return;
        float angle = 0f;
        if (!GameManager.Instance.IsThereAGamepadConnected || enemyIA.enabled)
        {
            angle = AngleBetweenPoints(target, transform.position);
        }
        else if (GameManager.Instance.IsThereAGamepadConnected)
        {
            if (target.x > 0.6f || target.x < -0.6f || target.y > 0.6f || target.y < -0.6f)
            {
                angle = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
                angle -= 90f;
                angle = flipRot ? -angle : angle;
            }
            else
            {
                return;
            }
        }

        if (angle < 0f)
        {
            angle += 360f;
        }

        float maxAngle = 180f;
        float minAngle = 0f;
        Vector3 up = transform.up;
        Vector3 right = transform.right;
        if (isClimbing)
        {
            if (wallCheckerL.isGrounded)
            {
                maxAngle -= 90f;
                minAngle -= 90f;
                up = transform.right;
                right = -transform.up;
            }
            else if(wallCheckerR.isGrounded)
            {
                maxAngle += 90f;
                minAngle += 90f;
                up = -transform.right;
                right = transform.up;
            }
            else
            {
                maxAngle += 180f;
                minAngle += 180f;
                up = -up;
                right = -right;
            }
        }
        if (Vector3.Cross(right, target - transform.position).z < 0f)
        {
            if (Vector3.Cross(up, target - transform.position).z < 0)
            {
                angle = minAngle;
            }
            else
            {
                angle = maxAngle;
            }
        }
        graphics.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public override void SetPossessingParameters()
    {
        base.SetPossessingParameters();

        CrystallineEnemyPossessingParameters crystallinePossessingParameters = (CrystallineEnemyPossessingParameters)possessingParameters;

        g = ((-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f))) * possessingParameters.gravityModifier;
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        cooldown = crystallinePossessingParameters.cooldown;
        windUp = crystallinePossessingParameters.windUp;
        stabDuration = crystallinePossessingParameters.stabDuration;

        GameManager.Instance.SetMusicSelectionIndex(5);
    }
}
