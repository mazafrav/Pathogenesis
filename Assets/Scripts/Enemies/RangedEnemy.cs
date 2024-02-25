using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private bool targetFound = false;
    public bool enablePatrolling = true;
    [SerializeField] private float detectionRange;
    private GameObject player;
    [SerializeField] private GameObject shootOrigin;
    private bool isAiming = false;
    [SerializeField] private float timeToShoot;
    [SerializeField] private float shootingCooldown;
    private bool canShoot = true;
    private bool isCancellingAggro = false;
    [SerializeField] private float timeToCancelAggro;
    [SerializeField] private GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (enablePatrolling)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            if (IsFacingRight())
            {
                rb.velocity = new Vector2(moveSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, 0);

            }

            float distance = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log(distance);
            if (distance <= detectionRange)
            {
                //RaycastHit2D hitResult = Physics2D.Linecast(shootOrigin.transform.position, player.transform.position, 1 << LayerMask.NameToLayer("Action"));
                RaycastHit2D hitResult = Physics2D.Raycast(shootOrigin.transform.position, (player.transform.position - shootOrigin.transform.position).normalized, detectionRange);
                Debug.DrawLine(shootOrigin.transform.position, player.transform.position, Color.red);
                if (hitResult.collider != null)
                {
                    Debug.Log(hitResult.collider.name);
                    if (hitResult.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("COLLIDING CON PLAYER");
                        enablePatrolling = false;
                    }
                }
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            RaycastHit2D hitResult = Physics2D.Raycast(shootOrigin.transform.position, (player.transform.position - shootOrigin.transform.position).normalized, detectionRange);
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

    private void Shoot()
    {
        Debug.Log("SHOOTING");
        Instantiate(bulletPrefab, shootOrigin.transform.position, Quaternion.identity);
    }

    private IEnumerator AimingPlayer()
    {
        isAiming = true;
        LineRenderer line = shootOrigin.GetComponent<LineRenderer>();
        line.enabled = true;
        line.startColor= Color.yellow;
        line.endColor = Color.yellow;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.SetPosition(0, shootOrigin.transform.position);
        line.SetPosition(1, player.transform.position);
        yield return new WaitForSeconds(timeToShoot);
        if (isAiming)
        {
            Shoot();
            StartCoroutine(ShootingCooldown(shootingCooldown));
        }
    }

    private IEnumerator ShootingCooldown(float cd)
    {
        canShoot = false;
        isAiming = false;
        LineRenderer line = shootOrigin.GetComponent<LineRenderer>();
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


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, detectionRange);
    }
}
