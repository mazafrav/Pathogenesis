using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedLocomotion : HostLocomotion
{
    [Header("Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shootOrigin;
    [SerializeField] public ParticleSystem chargeShotVFX;
    //private GroundChecker groundChecker;
    private float shootCooldown, windUp;
    public float shootCDTimer = 0.0f, windUpTimer = 0.0f;
    private ShootingComponent shootingComponent;

    private SpriteRenderer spriteRenderer;
    public Color defaultColor;
    [SerializeField]
    public Color colorWhileWindUp, colorWhileCooldown;
    private HostAbsorption absorption;

    [Header("SFX")]
    [SerializeField]
    private AudioClip shotClip;


    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        shootingComponent = GetComponentInChildren<ShootingComponent>();
        rb2D = GetComponentInParent<Rigidbody2D>();
        //groundChecker = GetComponentInChildren<GroundChecker>();
        shootCooldown = GetComponent<RangedEnemy>().playerShootingCooldown;
        windUp = GetComponent<RangedEnemy>().playerWindUp;
        spriteRenderer = GetComponent<RangedEnemy>().graphics.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        absorption = GetComponent<HostAbsorption>();


        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

    }

    // Update is called once per frame
    void Update()
    {
        //if (playerShot)
        //{
        //    shootCDTimer = Mathf.Max(shootCDTimer - Time.deltaTime, 0f);

        //}

        if (shootCDTimer <= 0f && windUpTimer > 0f)
        {
            windUpTimer = Mathf.Max(windUpTimer - Time.deltaTime, 0f);

            ChangeSpritesColor(Color.Lerp(GetCurrentColor(), colorWhileWindUp, 1.0f - windUpTimer));
            //Debug.Log("Wind up: " + currentWindUpTime);
            if (windUpTimer <= 0f)
            {
                Shoot(shootingComponent.transform.up, shootCDTimer);
            }
        }
        else if (shootCDTimer > 0f)
        {
            shootCDTimer = Mathf.Max(shootCDTimer - Time.deltaTime, 0f);

            ChangeSpritesColor(Color.Lerp(colorWhileCooldown, GetCurrentColor(), 1.0f - shootCDTimer));
        }
    }

    public override void Attack(Vector3 target = default)
    {
        if (!IsAttackReady()) { return; }

        chargeShotVFX.Play();

        if (GetComponentInParent<PlayerController>() != null )
        {
            windUpTimer = windUp;
        }
        else
        {
            Shoot(target);
        }
    }

    public override bool IsAttackReady()
    {
        return shootCDTimer <= 0.0f && windUpTimer <= 0.0f;
    }

    public override void Jump(float deltaX)
    {
        if (groundChecker.isGrounded)
        {
            rb2D.velocity = new Vector2(moveSpeed * deltaX, velocityY);
        }
    }

    public override void Move(float deltaX, float deltaY = 0)
    {
        rb2D.velocity = new Vector2(deltaX * moveSpeed, rb2D.velocity.y);
    }

    public override void ResetAttack()
    {
        chargeShotVFX.Stop();
        shootCDTimer = 0.0f;
        windUpTimer = 0.0f;
    }

    private void Shoot(Vector3 target, float cooldown = -1.0f)
    {
        chargeShotVFX.Stop();
        GetOneShotSource().PlayOneShot(shotClip);
        GameObject bullet = Instantiate(bulletPrefab, shootOrigin.transform.position, Quaternion.Euler(0, 0, 0));
        Vector3 direction = shootingComponent.transform.up;       
        // float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        // Quaternion.LookRotation()
        // bullet.transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        bullet.GetComponent<Bullet>().owner = gameObject;
        bullet.transform.up = direction.normalized;

        if (cooldown >= 0.0f) 
        {
            shootCDTimer = shootCooldown;
        }
    }

    public void ChangeSpritesColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
    public Color GetCurrentColor() { return transform.parent != null ? absorption.possessingColor : defaultColor; }

    public override void Aim(Vector3 target = default)
    {
        shootingComponent.Aim(target);
    }

    public override void SetPossessingParameters()
    {
        base.SetPossessingParameters();

        RangedEnemyPossessingParameters rangedPossessingParameters = (RangedEnemyPossessingParameters)possessingParameters;

        g = ((-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f))) * possessingParameters.gravityModifier;
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        windUp = rangedPossessingParameters.windUp;
        shootCooldown = rangedPossessingParameters.shootCooldown;

        GameManager.Instance.SetMusicSelectionIndex(3);

    }
}
