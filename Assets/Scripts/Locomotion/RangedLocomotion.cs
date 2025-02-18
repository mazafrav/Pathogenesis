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
    [SerializeField]
    private SpriteRenderer weaponSprite;
    private HostAbsorption absorption;

    private float originalY;
    private float heightJumped;
    private RangedEnemy rangedEnemy;

    [Header("SFX")]
    [SerializeField]
    private string windUpSFXPath = "event:/SFX/Enemies/Photogenic Charge";
    public FMOD.Studio.EventInstance windUpEventInstance;
    [SerializeField]
    private float distanceToFallToPlayLandClip = 2f;

    [Header("Lights")]
    [SerializeField] GameObject ligthSource;
    [SerializeField] GameObject possessedLightSource;
    public delegate void OnShoot();
    public OnShoot onShoot;

    [Header("Animation")]
    [SerializeField] public Animator rangedAnimator;

    // Start is called before the first frame update
    void Start()
    {
        rangedEnemy = GetComponent<RangedEnemy>();
        shootingComponent = GetComponentInChildren<ShootingComponent>();
        rb2D = GetComponentInParent<Rigidbody2D>();
        //groundChecker = GetComponentInChildren<GroundChecker>();
        shootCooldown = rangedEnemy.playerShootingCooldown;
        windUp = rangedEnemy.playerWindUp;
        spriteRenderer = rangedEnemy.graphics.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        absorption = GetComponent<HostAbsorption>();


        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

        //GetOneShotSource().pitch += 0.5f;

        if (windUpSFXPath.Equals(""))
        {
            windUpSFXPath = "event:/SFX/Enemies/Photogenic Charge";
    }
        windUpEventInstance = FMODUnity.RuntimeManager.CreateInstance(windUpSFXPath);
        //jumpEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        //landEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerShot)
        //{
        //    shootCDTimer = Mathf.Max(shootCDTimer - Time.deltaTime, 0f);

        //}

        //if (debug)
        //{
        //    //GameManager.Instance.Getpla
        //    Debug.Log("Ranged Position is:" + transform.position.ToString());

        //    jumpEventInstance.get3DAttributes(out FMOD.ATTRIBUTES_3D attributes);
        //    Debug.Log("Position: " + attributes.position.x + ", " + attributes.position.y);
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

        if (groundChecker.isGrounded)
        {
            originalY = transform.position.y;
        }
        else
        {
            float yDiff = Mathf.Abs(transform.position.y - originalY);
            heightJumped = Mathf.Max(heightJumped, yDiff);
        }

        if (heightJumped >= distanceToFallToPlayLandClip && groundChecker.isGrounded)
        {
            heightJumped = 0f;
            landEventInstance.start();
        }
    }

    public override void Attack(Vector3 target = default)
    {
        if (!IsAttackReady()) { return; }

        chargeShotVFX.Play();
        
        if (rangedEnemy.IsPossesed)
        {
            rangedAnimator.Play("RangedEnemAttack");
            windUpTimer = windUp;
            windUpEventInstance.start();
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
            jumpEventInstance.start();
            heightJumped = 0f;
        }
    }

    public override void JumpCancel() { }

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

        //GetOneShotSource().pitch -= 0.5f;
        //GetOneShotSource().PlayOneShot(shotClip);
        //GetOneShotSource().pitch += 0.5f;

        attackEventInstance.start();

        GameObject bullet = Instantiate(bulletPrefab, shootOrigin.transform.position, Quaternion.Euler(0, 0, 0));
        Vector3 direction = shootingComponent.transform.up;       
        // float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        // Quaternion.LookRotation()
        // bullet.transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        bullet.GetComponent<Bullet>().Owner = gameObject;
        bullet.transform.up = direction.normalized;

        onShoot?.Invoke();

        if (cooldown >= 0.0f) 
        {
            shootCDTimer = shootCooldown;
        }
    }

    public void ChangeSpritesColor(Color newColor)
    {
        spriteRenderer.color = newColor;
        weaponSprite.color = newColor;
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

        //GameManager.Instance.SetMusicSelectionIndex(3);

        //change light source
        ligthSource.SetActive(false);
        possessedLightSource.SetActive(true);

    }

    public override void SetMoveSpeed(float newSpeed)
    {
        base.SetMoveSpeed(newSpeed);
        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);
    }

    public override void CancelAttack()
    {
        
    }
}
