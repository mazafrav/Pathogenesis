using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    private Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] private RangedLocomotion locomotion;
    [SerializeField] public GameObject graphics;
    private bool enablePatrolling = true;

    [Header("Attack")]
    [SerializeField] private float detectionRange;
    //private GameObject player;
    [SerializeField] private GameObject shootDetection;
    [SerializeField] private GameObject bulletSpawner;
    private bool isAiming = false;
    [SerializeField] private float timeToShoot; //a.k.a. Windup for AI
    private float timeToShootTimer;
    [SerializeField] public float playerWindUp;
    [SerializeField] public float shootingCooldown;
    private float shootingCooldownTimer;
    [SerializeField] public float playerShootingCooldown;
    private bool canShoot = true;
    [SerializeField]
    public ShootingComponent shootingComponent;

    float movementDirection = -1;
    [SerializeField]
    private RangedEnemyDetection rangedEnemyDetection;
    private bool isSeeing = false;
    private Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetComponent<Collider2D>());
        rangedEnemyDetection.transform.localScale = new Vector3(detectionRange * 2.0f, detectionRange * 2.0f, rangedEnemyDetection.transform.localScale.z);

    }

    void Update()
    {
        // if(targetPosition != null)
        // {
        //     locomotion.Aim(targetPosition);
        // }
        // Windup color
        if (shootingCooldownTimer <= 0f && timeToShootTimer > 0f)
        {
            timeToShootTimer = Mathf.Max(timeToShootTimer - Time.deltaTime, 0f);

            locomotion.ChangeSpritesColor(Color.Lerp(locomotion.GetCurrentColor(), locomotion.colorWhileWindUp, 1.0f - timeToShootTimer));
            if (timeToShootTimer <= 0f)
            {
                ActivateCD();
            }
        }
        // Cooldown color
        else if (shootingCooldownTimer > 0f)
        {
            shootingCooldownTimer = Mathf.Max(shootingCooldownTimer - Time.deltaTime, 0f);

            locomotion.ChangeSpritesColor(Color.Lerp(locomotion.colorWhileCooldown, locomotion.GetCurrentColor(), 1.0f - shootingCooldownTimer));

        }

        // Patrolling behaviour. This means the Ranged Enemy hasn't found any target to shoot
        if (enablePatrolling)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (wayPoints.Length != 0)
            {
                if (transform.position.x >= wayPoints[0].position.x)
                {
                    graphics.transform.rotation = Quaternion.Euler(0, 0, 90);
                    movementDirection = -1;
                }

                else if (transform.position.x < wayPoints[1].position.x)
                {
                    movementDirection = 1;
                    graphics.transform.rotation = Quaternion.Euler(0, 0, -90);
                }
                locomotion.Move(movementDirection);
            }

            // Search target selects the closest visible target and stops the patrolling behaviour
            rangedEnemyDetection.targetInRange = SearchTarget();
            
        }

        // Shooting behaviour. Target is not null
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            locomotion.Move(0);

            // When a possible target leaves the ranged enemy's detection area, it's removed from the "allTargetsInRange" array.
            // Hence, we need to check if its current target has left its detection area.
            if (!rangedEnemyDetection.allTargetsInRange.Contains(rangedEnemyDetection.targetInRange))
            {
                // We search once again for its closest visible target. If there is none, patrolling behaviour is restored.
                rangedEnemyDetection.targetInRange = SearchTarget();          
            }
            else
            {
                // We check if there is any obstacle between the ranged enemy and its target.
                // We do this by seeing if there is any tile map closer to the enemy than its target.
                RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, (rangedEnemyDetection.targetInRange.transform.position - transform.position).normalized, detectionRange);
                for (int i = 0; i < raycastHit2D.Length; i++)
                {
                    if (raycastHit2D[i].collider.CompareTag("TileMap") &&
                        (Vector2.Distance(raycastHit2D[i].point, transform.position) < Vector2.Distance(rangedEnemyDetection.targetInRange.transform.position, transform.position)))
                    {
                        isSeeing = false;
                        break;
                    }
                    else if (raycastHit2D[i].collider.gameObject == rangedEnemyDetection.targetInRange)
                    {
                        Debug.DrawRay(transform.position, (rangedEnemyDetection.targetInRange.transform.position - transform.position).normalized * raycastHit2D[i].distance, Color.red);
                        isSeeing = true;
                        
                    }
                }

                // If target is visible, start aiming and shoot.
                // If it's not, look for another target. If there is none, enable patrolling once again.
                CheckTargetIsVisible();
                
            }           
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    // Searches for closest visible target. If there is none, go back to patrolling
    private GameObject SearchTarget()
    {
        GameObject result = null;

        // If there are any possible targets in range, they will be stored in this "allTargetsInRange" array
        if (rangedEnemyDetection.allTargetsInRange.Count != 0)
        {
            // Check if Ranged can aim any of the possible targets
            foreach (var target in rangedEnemyDetection.allTargetsInRange)
            {
                RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, (target.transform.position - transform.position).normalized, detectionRange);
                for (int i = 0; i < raycastHit2D.Length; i++)
                {
                    if (raycastHit2D[i].collider.gameObject == target)
                    {
                        enablePatrolling = false;
                        result = target;
                        Debug.Log("MI TARGET ES: " + result.gameObject.name);
                        break;
                    }
                    else if (raycastHit2D[i].collider.gameObject.CompareTag("TileMap"))
                    {
                        enablePatrolling = true;
                        break;
                    }
                }
            }
        }
        else
        {
            enablePatrolling = true;
        }
        return result;
    }

    private void CheckTargetIsVisible()
    {
        if (isSeeing)
        {
            // If Ranged is seeing the target, start aiming
            if (canShoot)
            {
                StartCoroutine(AimRoutine());
            }
            // While aiming AND seeing the target, store its position.
            // In case of losing sight of the target, it will shoot at its last known position
            if (isAiming)
            {
                targetPosition = rangedEnemyDetection.targetInRange.transform.position;
                locomotion.Aim(targetPosition);
                // if (rangedEnemyDetection.targetInRange.transform.position.x <= transform.position.x)
                // {
                //     graphics.transform.rotation = Quaternion.Euler(0, 0, 90);
                // }
                // else
                // {
                //     graphics.transform.rotation = Quaternion.Euler(0, 0, -90);

                // }
            }
        }
        else
        {
            // If not seeing and not aiming, look for another target.
            // If there is no target, patrolling behaviour will be restored.
            if (!isAiming)
            {
                rangedEnemyDetection.targetInRange = SearchTarget();
            }
        }
    }

    private IEnumerator AimRoutine()
    {
        isAiming = true;
        canShoot = false;
        timeToShootTimer = timeToShoot;

        yield return new WaitForSeconds(timeToShoot);

        locomotion.Attack(targetPosition);
        StartCoroutine(ShootingCooldownRoutine(shootingCooldown));
    }
    

    private IEnumerator ShootingCooldownRoutine(float cd)
    {
        //canShoot = false;
        isAiming = false;
        yield return new WaitForSeconds(cd);
        canShoot = true;
    }

    // Used for when possessing
    public void ResetRigidbodyConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Used for when possessing
    public void SetAimBehaviour(bool value)
    {
        shootingComponent.bisActive = value;
    }

    private void ActivateCD()
    {
        shootingCooldownTimer = shootingCooldown;
    }

}
