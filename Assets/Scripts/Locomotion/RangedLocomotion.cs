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
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponentInParent<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        shootCooldown = GetComponent<RangedEnemy>().playerShootingCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerShot)
        {
            shootCDTimer = Mathf.Max(shootCDTimer - Time.deltaTime, 0f);

        }
    }

    public override void Attack(float rotation = 0.0f)
    {
        if (!IsAttackReady()) { return; }
        playerShot = false;
        Instantiate(bulletPrefab, shootOrigin.transform.position, Quaternion.Euler(0, 0, rotation + 90));

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
            rb2D.velocity = new Vector2(moveSpeed * deltaX, 7.5f);
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
