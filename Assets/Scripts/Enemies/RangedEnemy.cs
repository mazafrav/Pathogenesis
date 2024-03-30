using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    private Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] private RangedLocomotion locomotion;
    [SerializeField] private GameObject graphics;
    private bool enablePatrolling = true;
    [Header("Shooting")]
    [SerializeField] private float detectionRange;
    //private GameObject player;
    [SerializeField] private GameObject shootDetection;
    [SerializeField] private GameObject bulletSpawner;
    private bool isAiming = false;
    [SerializeField] private float timeToShoot;
    [SerializeField] public float shootingCooldown;
    [SerializeField] public float playerShootingCooldown;
    private bool canShoot = true;
    private bool isCancellingAggro = false;
    [SerializeField] private float timeToCancelAggro;
    public GameObject player = null;
    [SerializeField]
    public ShootingComponent shootingComponent;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            return;
        }

        if (enablePatrolling)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (IsFacingRight())
            {
                graphics.transform.rotation = Quaternion.Euler(0,0, -90);
                locomotion.Move(1);
            }
            else
            {
                graphics.transform.rotation = Quaternion.Euler(0, 0, 90);
                locomotion.Move(-1);

            }
           
            float distance = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log(distance);
            if (distance <= detectionRange)
            {
                //RaycastHit2D hitResult = Physics2D.Linecast(shootOrigin.transform.position, player.transform.position, 1 << LayerMask.NameToLayer("Action"));
                RaycastHit2D hitResult = Physics2D.Raycast(shootDetection.transform.position, (player.transform.position - shootDetection.transform.position).normalized, detectionRange);
                Debug.DrawLine(shootDetection.transform.position, player.transform.position, Color.red);
                if (hitResult.collider != null)
                {
                    Debug.Log(hitResult.collider.name);
                    if (hitResult.collider.gameObject.CompareTag("Player"))
                    {
                        enablePatrolling = false;
                    }
                }
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            locomotion.Move(0);
            float distance = Vector3.Distance(transform.position, player.transform.position);
            RaycastHit2D hitResult = Physics2D.Raycast(shootDetection.transform.position, (player.transform.position - shootDetection.transform.position).normalized, detectionRange);
            if (hitResult.collider != null)
            {
                if (distance <= detectionRange && hitResult.collider.gameObject.CompareTag("Player"))
                {
                    StopCoroutine(CancelAggro());
                    isCancellingAggro = false;
                    if (canShoot)
                    {
                        StartCoroutine(AimingPlayer());
                    }
                    if (isAiming)
                    {
                        LineRenderer line = shootDetection.GetComponent<LineRenderer>();
                        line.enabled = true;
                        line.startColor = Color.yellow;
                        line.endColor = Color.yellow;
                        line.startWidth = 0.1f;
                        line.endWidth = 0.1f;
                        line.SetPosition(0, bulletSpawner.transform.position);
                        line.SetPosition(1, player.transform.position);
                    }

                }
                else
                {
                    CancelAiming();
                    if (!isCancellingAggro)
                    {
                        StartCoroutine(CancelAggro());
                    }
                }
            }
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private IEnumerator AimingPlayer()
    {
        isAiming = true;
        canShoot = false;
        yield return new WaitForSeconds(timeToShoot);
        if (isAiming)
        {
            locomotion.Attack(player.transform.position);
            StartCoroutine(ShootingCooldown(shootingCooldown));
        }
    }

    private IEnumerator ShootingCooldown(float cd)
    {
        //canShoot = false;
        isAiming = false;
        LineRenderer line = shootDetection.GetComponent<LineRenderer>();
        line.enabled = false;
        yield return new WaitForSeconds(cd);
        canShoot = true;
    }

    private void CancelAiming()
    {
        if (isAiming)
        {
            StopCoroutine(AimingPlayer());
            StartCoroutine(ShootingCooldown(shootingCooldown / 2));
        }
    }

    private IEnumerator CancelAggro()
    {
        isCancellingAggro = true;
        yield return new WaitForSeconds(timeToCancelAggro);
        if (isCancellingAggro)
        {
            enablePatrolling = true;
            isCancellingAggro = false;
        }
    }

    public void ResetRigidbodyConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetAimBehaviour(bool value)
    {
        shootingComponent.bisActive = value;
    }

    //void OnDrawGizmosSelected()
    //{
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(transform.position, detectionRange);
    //}

}
