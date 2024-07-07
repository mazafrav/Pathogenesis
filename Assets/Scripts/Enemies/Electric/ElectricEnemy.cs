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
    private ElectricLocomotion electricLocomotion = null;

    private bool isSeeingTarget = false, isPatrolling = true, canAttackTarget = false;
    
    public Vector2 direction { get; set; } = Vector2.zero;
    public bool ISeeingTarget() { return isSeeingTarget; }

    void Start()
    {
        electricLocomotion = GetComponent<ElectricLocomotion>();
        followRange = GetComponentInChildren<ElectricFollowRange>();
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        if (followRange.chosenTarget) //We are seeing the target
        {
            CheckIfDetected(followRange.chosenTarget);
            electricLocomotion.SetMoveSpeed(chaseSpeed);
            isSeeingTarget = true;
            isPatrolling = false;
        }
        else
        {
            isPatrolling = true;
            isSeeingTarget = false;
        }
    
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
