using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedLocomotion : HostLocomotion
{
    [Header("Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shootOrigin;
    private GroundChecker groundChecker;
    private bool playerShot = false;
    private float shootCooldown, windUp;
    private float shootCDTimer = 0.0f, windUpTimer = 0.0f;
    private Vector3 target;

    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponentInParent<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        shootCooldown = GetComponent<RangedEnemy>().playerShootingCooldown;
        windUp = GetComponent<RangedEnemy>().playerWindUp;

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

            //ChangeSpritesColor(Color.Lerp(GetCurrentColor(), colorWhileWindUp, 1.0f - currentWindUpTime));
            //Debug.Log("Wind up: " + currentWindUpTime);
            if (windUpTimer <= 0f)
            {
                Shoot(target, shootCDTimer);
            }
        }
        else if (shootCDTimer > 0f)
        {
            shootCDTimer = Mathf.Max(shootCDTimer - Time.deltaTime, 0f);
        }
    }

    public override void Attack(Vector3 target = default)
    {
        if (!IsAttackReady()) { return; }

        if (GetComponentInParent<PlayerController>() != null )
        {
            this.target = target;
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
        shootCDTimer = 0.0f;
        windUpTimer = 0.0f;
    }

    private void Shoot(Vector3 target, float cooldown = -1.0f)
    {
        GameObject bullet = Instantiate(bulletPrefab, shootOrigin.transform.position, Quaternion.Euler(0, 0, 0));
        Vector3 direction = target - bullet.transform.position;
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        if (cooldown >= 0.0f) 
        {
            shootCDTimer = shootCooldown;
        }
    }
}
