using System;
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

    [Header("Attack")]
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

    float movementDirection = -1;
    [SerializeField]
    private RangedEnemyDetection rangedEnemyDetection;
    private bool isSeeing = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetComponent<Collider2D>());
        rangedEnemyDetection.transform.localScale = new Vector3(detectionRange * 2.0f, detectionRange * 2.0f, rangedEnemyDetection.transform.localScale.z);

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
            //if (IsFacingRight())
            //{
            //    graphics.transform.rotation = Quaternion.Euler(0,0, -90);
            //    locomotion.Move(1);
            //}
            //else
            //{
            //    graphics.transform.rotation = Quaternion.Euler(0, 0, 90);
            //    locomotion.Move(-1);

            //}

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


            float distance = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log(distance);
            if (distance <= detectionRange)

           
            
            if (rangedEnemyDetection.personInRange != null)

            {
                //RaycastHit2D hitResult = Physics2D.Linecast(shootOrigin.transform.position, player.transform.position, 1 << LayerMask.NameToLayer("Action"));
                //RaycastHit2D hitResult = Physics2D.Raycast(shootDetection.transform.position, (player.transform.position - shootDetection.transform.position).normalized, detectionRange);
                //Debug.DrawLine(shootDetection.transform.position, player.transform.position, Color.red);
                //if (hitResult.collider != null)
                //{
                //    Debug.Log(hitResult.collider.name);
                //    if (hitResult.collider.gameObject.CompareTag("Player"))
                //    {
                //        enablePatrolling = false;
                //    }
                //}

                RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, (rangedEnemyDetection.personInRange.transform.position - transform.position).normalized, detectionRange);
                for (int i = 0; i < raycastHit2D.Length; i++)
                {
                    if (raycastHit2D[i].collider.gameObject == rangedEnemyDetection.personInRange)
                    {
                        enablePatrolling = false;
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
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            locomotion.Move(0);

            if (rangedEnemyDetection.personInRange == null)
            {
                CancelAiming();
                if (!isCancellingAggro)
                {
                    StartCoroutine(CancelAggro());
                }
            }
            else
            {
                RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, (rangedEnemyDetection.personInRange.transform.position - transform.position).normalized, detectionRange);
                //Array.Sort(raycastHit2D, delegate (RaycastHit2D hit1, RaycastHit2D hit2)
                //{
                //    return Vector2.Distance(hit1.collider.transform.position, transform.position).CompareTo(Vector2.Distance(hit1.collider.transform.position, transform.position));
                //});
                for (int i = 0; i < raycastHit2D.Length; i++)
                {
                    if (raycastHit2D[i].collider.CompareTag("TileMap") &&
                        (Vector2.Distance(raycastHit2D[i].collider.transform.position, transform.position) < Vector2.Distance(rangedEnemyDetection.personInRange.transform.position, transform.position)))
                    {
                        isSeeing = false;
                        break;
                    }
                    else if (raycastHit2D[i].collider.gameObject == rangedEnemyDetection.personInRange)
                    {
                        Debug.DrawRay(transform.position, (rangedEnemyDetection.personInRange.transform.position - transform.position).normalized * raycastHit2D[i].distance, Color.red);
                        isSeeing = true;
                        
                    }
                    //Debug.Log("Elemento golpeado: " + raycastHit2D[i].collider.gameObject.name + " con distancia " + Vector2.Distance(raycastHit2D[i].collider.transform.position, transform.position));

                }

                if (isSeeing)
                {
                    StopCoroutine(CancelAggro());
                    isCancellingAggro = false;
                    if (canShoot)
                    {
                        StartCoroutine(AimingPlayer());
                    }
                    if (isAiming)
                    {
                        RaycastHit2D[] pitoteRayCast = Physics2D.RaycastAll(bulletSpawner.transform.position, (rangedEnemyDetection.personInRange.transform.position - bulletSpawner.transform.position).normalized, detectionRange);
                        //Array.Sort(raycastHit2D, delegate (RaycastHit2D hit1, RaycastHit2D hit2)
                        //{
                        //    return Vector2.Distance(hit1.collider.transform.position, transform.position).CompareTo(Vector2.Distance(hit1.collider.transform.position, transform.position));
                        //});
                        for (int i = 0; i < pitoteRayCast.Length; i++)
                        {
                            if (pitoteRayCast[i].collider.CompareTag("TileMap") &&
                                (Vector2.Distance(pitoteRayCast[i].collider.transform.position, transform.position) < Vector2.Distance(rangedEnemyDetection.personInRange.transform.position, transform.position)))
                            {
                                if (IsFacingRight())
                                {
                                    graphics.transform.rotation = Quaternion.Euler(0, 0, 90);
                                    locomotion.Move(1);
                                }
                                else
                                {
                                    graphics.transform.rotation = Quaternion.Euler(0, 0, -90);
                                    locomotion.Move(-1);

                                }
                                break;
                            }
                            //Debug.Log("Elemento golpeado: " + raycastHit2D[i].collider.gameObject.name + " con distancia " + Vector2.Distance(raycastHit2D[i].collider.transform.position, transform.position));

                        }
                        LineRenderer line = GetComponent<LineRenderer>();
                        line.enabled = true;
                        line.startWidth = 0.1f;
                        line.endWidth = 0.1f;
                        line.SetPosition(0, bulletSpawner.transform.position);
                        line.SetPosition(1, rangedEnemyDetection.personInRange.transform.position);
                    }
                }
            }
            //float distance = Vector3.Distance(transform.position, player.transform.position);
            //RaycastHit2D hitResult = Physics2D.Raycast(shootDetection.transform.position, (player.transform.position - shootDetection.transform.position).normalized, detectionRange);
            //if (hitResult.collider != null)
            //{
            //    if (distance <= detectionRange && hitResult.collider.gameObject.CompareTag("Player"))
            //    {
            //        StopCoroutine(CancelAggro());
            //        isCancellingAggro = false;
            //        if (canShoot)
            //        {
            //            StartCoroutine(AimingPlayer());
            //        }
            //        if (isAiming)
            //        {
            //            LineRenderer line = shootDetection.GetComponent<LineRenderer>();
            //            line.enabled = true;
            //            line.startColor = Color.yellow;
            //            line.endColor = Color.yellow;
            //            line.startWidth = 0.1f;
            //            line.endWidth = 0.1f;
            //            line.SetPosition(0, bulletSpawner.transform.position);
            //            line.SetPosition(1, player.transform.position);
            //        }

            //    }
            //    else
            //    {
            //        CancelAiming();
            //        if (!isCancellingAggro)
            //        {
            //            StartCoroutine(CancelAggro());
            //        }
            //    }
            //}
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
        LineRenderer line = GetComponent<LineRenderer>();
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
