using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedLocomotion : HostLocomotion
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shootOrigin;
    private GroundChecker groundChecker;
    private bool playerShot = false;
    private float shootCooldown;
    private float shootCDTimer = 0;

    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponentInParent<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        shootCooldown = GetComponent<RangedEnemy>().playerShootingCooldown;

        jumpDistance += jumpOffset;
        jumpHeight += jumpOffset;

        g = (-2 * jumpHeight * moveSpeed * moveSpeed) / ((jumpDistance / 2.0f) * (jumpDistance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * jumpHeight * moveSpeed) / (jumpDistance / 2.0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (playerShot)
        {
            shootCDTimer = Mathf.Max(shootCDTimer - Time.deltaTime, 0f);

        }
    }

    public override void Attack(Vector3 target = default)
    {
        if (!IsAttackReady()) { return; }
        playerShot = false;
        GameObject bullet = Instantiate(bulletPrefab, shootOrigin.transform.position, Quaternion.Euler(0, 0, 0));
        Vector3 direction = target - bullet.transform.position;
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        if (GetComponentInParent<PlayerController>() != null )
        {
            playerShot = true;
            shootCDTimer = shootCooldown;
        }
    }

    public override bool IsAttackReady()
    {
        return shootCDTimer <= 0.0f;
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
        shootCDTimer = 0f;
    }
}
