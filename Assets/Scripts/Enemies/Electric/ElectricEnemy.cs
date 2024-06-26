using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ElectricEnemy : Enemy
{

    [Header("Movement")]
    [SerializeField]
    private float stoppingDistance = 10.0f;


    private ElectricFollowRange followRange = null;
    //[SerializeField]
    //private ElectricAttackRange attackRange = null;

    public Vector2 direction { get; set; } = Vector2.zero;
    private bool isSeeingTarget = false, isPatrolling = true, canAttackTarget = false;
    
    public bool ISeeingTarget() { return isSeeingTarget; }

    //private ElectricLocomotion electricLocomotion;


    void Start()
    {
        followRange = GetComponentInChildren<ElectricFollowRange>();
        //electricLocomotion = (ElectricLocomotion)locomotion;
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {

        //if (followRange.personInRange) //We check if we see the target in the follow range (player or other enemies)
        //{
        //    direction = (followRange.personInRange.transform.position - transform.position).normalized;

        //    RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, direction, 2*electricLocomotion.FollowRange);
        //    for (int i = 0; i < raycastHit2D.Length; i++)
        //    {                                                                          //Electric enemies dont attack electric enemies
        //        if (raycastHit2D[i].collider.gameObject == followRange.personInRange && followRange.personInRange.GetComponent<ElectricEnemy>() == null)
        //        {
        //            isSeeingTarget = true;
        //            Debug.DrawRay(transform.position, direction * raycastHit2D[i].distance, Color.red);
        //            break;
        //        }
        //        else if (raycastHit2D[i].collider.gameObject.CompareTag("TileMap"))
        //        {
        //            isSeeingTarget = false;
        //            break;
        //        }
        //    }
        //}
        //else
        //{
        //    isSeeingTarget = false;
        //}

        if (followRange.chosenTarget) //We are seeing the target
        {
            isSeeingTarget = true;
            isPatrolling = false;
        }
        else
        {
            isPatrolling = true;
            isSeeingTarget = false;
        }

        //if (isSeeingTarget) 
        //{
        //    isPatrolling = false;
        //}
        //else //We are not seeing the target
        //{
        //    isPatrolling = true;
        //    isSeeingTarget = false;
        //}     
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            if (wayPoints.Length != 0)
            {
                Patrol();             
            }
         
        }
        else if (isSeeingTarget && followRange.chosenTarget)
        {
            float dis = Vector2.Distance(followRange.chosenTarget.transform.position, transform.position);
            if (dis > stoppingDistance)
            {
                locomotion.Move(direction.x);
            }
        }
    }
}
