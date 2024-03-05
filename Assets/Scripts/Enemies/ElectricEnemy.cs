using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ElectricEnemy : Enemy
{

    [Header("Movement")]
    [SerializeField]
    ElectricLocomotion locomotion;
    [SerializeField]
    private float stoppingDistance = 10.0f;

    [SerializeField]
    private LayerMask playerLayerMask;

    private GameObject player = null;

    //private Rigidbody2D rb2D = null;

    //private bool isActiveElectricShock = true;
    //private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    //private float currentCooldownTime = 0.0f, currentWindUpTime = 0.5f;

    private Vector2 direction = Vector2.zero;
    private bool isFinishedWindUp = false, isSeeingPlayer = false, isPatrolling = true;

    void Start()
    {
        //Habria q hacerlo x game manager probablemente
        player = GameObject.Find("Player");
    }

    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, 200, playerLayerMask);
        if(Vector2.Distance(player.transform.position, transform.position) <= locomotion.ElectricShockRange &&
            raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player"))
        {
            isSeeingPlayer = true;
            isPatrolling = false;
            locomotion.IsSeeingPlayer = true;
            locomotion.Attack();
            Debug.Log("Seeing player");
            Debug.DrawRay(transform.position, direction * raycastHit2D.distance, Color.red);
        }
        else
        {
            isPatrolling = true;
            locomotion.IsSeeingPlayer = false;
            locomotion.ResetAttack();
            Debug.Log("Not seeing player");
        }
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            if (IsFacingRight())
            {
                locomotion.Move(1);
            }
            else
            {
                locomotion.Move(-1);
            }
        }
        else
        {
            if (isSeeingPlayer)
            {
                if (locomotion.hasFinishedWindUp)
                {
                    float dis = Vector2.Distance(player.transform.position, transform.position);
                    if (dis > stoppingDistance)
                    {
                        locomotion.Move(direction.x);
                        // rb2D.velocity = new Vector2(direction.x * speed, 0.0f);
                    }
                }
                else
                {
                    Debug.Log("Not moving during wind up");
                }
            }
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
}
