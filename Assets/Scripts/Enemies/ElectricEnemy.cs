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

    [Header("Ray detection layer mask")]
    [SerializeField]
    private LayerMask rayLayerMask;

    private GameObject player = null;

    private Vector2 direction = Vector2.zero;
    private bool isSeeingPlayer = false, isPatrolling = true;

    void Start()
    {
        //Habria q hacerlo x game manager probablemente
        player = GameObject.Find("Player");     
    }

    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        //RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(transform.position, direction, locomotion.ElectricShockRange);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, locomotion.ElectricShockRange, rayLayerMask);
        if(Vector2.Distance(player.transform.position, transform.position) <= locomotion.ElectricShockRange &&
            raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player")) //We are seeing the player
        {
            Debug.Log(raycastHit2D.collider.name);
            isSeeingPlayer = true;
            isPatrolling = false;
            locomotion.Attack();            
            //Debug.Log("Seeing player");
            Debug.DrawRay(transform.position, direction * raycastHit2D.distance, Color.red);
        }
        else //We are not seeing the player
        {
            isPatrolling = true;
            isSeeingPlayer = false;
            locomotion.ResetAttack();
            //Debug.Log("Not seeing player");
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
        else if(isSeeingPlayer)
        {                                        
            if(locomotion.IsWindingUp() && locomotion.IsCooldownFinished())
            {
                Debug.Log("Not moving during wind up");
                locomotion.Move(0);
            }
            else
            {
                Debug.Log("Moving towards player");
                float dis = Vector2.Distance(player.transform.position, transform.position);
                if (dis > stoppingDistance)
                {
                    locomotion.Move(direction.x);
                    // rb2D.velocity = new Vector2(direction.x * speed, 0.0f);
                }
            }
        }
    }

    

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
}
