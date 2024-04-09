using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ElectricEnemy : Enemy
{

    [Header("Movement")]
    [SerializeField]
    ElectricLocomotion locomotion;
    [SerializeField]
    private float stoppingDistance = 10.0f;

    private ElectricShockRange shockRange = null;

    private Vector2 direction = Vector2.zero;
    private bool isSeeingTarget = false, isPatrolling = true;
    private float movementDirection = 1.0f;

    void Start()
    {
        shockRange = GetComponentInChildren<ElectricShockRange>();
        shockRange.transform.localScale = new Vector3(locomotion.ElectricShockRange * 2.0f, locomotion.ElectricShockRange * 2.0f, shockRange.transform.localScale.z);
    }

    void Update()
    {

        if (shockRange.personInRange) //We check if we see the target in range (player or other enemies)
        {
            direction = (shockRange.personInRange.transform.position - transform.position).normalized;

            RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, direction, locomotion.ElectricShockRange/*, rayLayerMask*/);
            for (int i = 0; i < raycastHit2D.Length; i++)
            {
                if (raycastHit2D[i].collider.gameObject == shockRange.personInRange)
                {
                    Debug.Log("Seeing");
                    isSeeingTarget = true;
                    Debug.DrawRay(transform.position, direction * raycastHit2D[i].distance, Color.red);
                    break;
                }
                else if (raycastHit2D[i].collider.gameObject.CompareTag("TileMap"))
                {
                    Debug.Log("Not seeing");
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
                if (transform.position.x >= wayPoints[0].position.x)
                {
                    movementDirection = -1;
                }
                else if (transform.position.x < wayPoints[1].position.x)
                {
                    movementDirection = 1;
                }
                locomotion.Move(movementDirection);
            }
            //if (IsFacingRight())
            //{
            //    locomotion.Move(1);
            //}
            //else
            //{
            //    locomotion.Move(-1);
            //}
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

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
}
