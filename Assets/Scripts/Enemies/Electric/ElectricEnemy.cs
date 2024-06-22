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

    private ElectricShockRange shockRange = null;

    private Vector2 direction = Vector2.zero;
    private bool isSeeingTarget = false, isPatrolling = true;
    
    private ElectricLocomotion electricLocomotion;


    void Start()
    {
        electricLocomotion = (ElectricLocomotion)locomotion;
        shockRange = GetComponentInChildren<ElectricShockRange>();
        shockRange.transform.localScale = new Vector3(electricLocomotion.ElectricShockRange, electricLocomotion.ElectricShockRange, shockRange.transform.localScale.z);
    }

    void Update()
    {

        if (shockRange.personInRange) //We check if we see the target in range (player or other enemies)
        {
            direction = (shockRange.personInRange.transform.position - transform.position).normalized;

            RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, direction, electricLocomotion.ElectricShockRange);
            for (int i = 0; i < raycastHit2D.Length; i++)
            {                                                                          //Electric enemies dont attack electric enemies
                if (raycastHit2D[i].collider.gameObject == shockRange.personInRange && shockRange.personInRange.GetComponent<ElectricEnemy>() == null)
                {
                    isSeeingTarget = true;
                    Debug.DrawRay(transform.position, direction * raycastHit2D[i].distance, Color.red);
                    break;
                }
                else if (raycastHit2D[i].collider.gameObject.CompareTag("TileMap"))
                {
                    isSeeingTarget = false;
                    break;
                }
            }
        }
        else
        {
            isSeeingTarget = false;
        }



        if (isSeeingTarget) //We are seeing the target
        {
            //isSeeingPlayer = true;
            isPatrolling = false;
            locomotion.Attack();
        }
        else //We are not seeing the target
        {
            isPatrolling = true;
            isSeeingTarget = false;
            //locomotion.ResetAttack();
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
        else if (isSeeingTarget && shockRange.personInRange)
        {
            float dis = Vector2.Distance(shockRange.personInRange.transform.position, transform.position);
            if (dis > stoppingDistance)
            {
                locomotion.Move(direction.x);
            }
        }
    }
}
