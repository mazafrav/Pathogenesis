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

    //[Header("Ray detection layer mask")]
    //[SerializeField]
    //private LayerMask rayLayerMask;

    //[SerializeField]
    //private GameObject player = null;

    private ElectricShockRange shockRange = null;

    private Vector2 direction = Vector2.zero;
    private bool isSeeingPlayer = false, isPatrolling = true;

    void Start()
    {
        shockRange = GetComponentInChildren<ElectricShockRange>();
    }

    void Update()
    {

        if (shockRange.personInRange)
        {
            direction = (shockRange.personInRange.transform.position - transform.position).normalized;

            RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, direction, locomotion.ElectricShockRange/*, rayLayerMask*/);
            for (int i = 0; i < raycastHit2D.Length; i++)
            {
                if (raycastHit2D[i].collider.gameObject == shockRange.personInRange)
                {
                    Debug.Log("Seeing");
                    isSeeingPlayer = true;
                    Debug.DrawRay(transform.position, direction * raycastHit2D[i].distance, Color.red);
                    break;
                }
                else if (raycastHit2D[i].collider.gameObject.CompareTag("TileMap"))
                {
                    Debug.Log("Not seeing");
                    isSeeingPlayer = false;
                    break;
                }
            }
        }
        else
        {
            isSeeingPlayer = false;
        }



        if (isSeeingPlayer/*Vector2.Distance(shockRange.personInRange.position, transform.position) <= locomotion.ElectricShockRange*/
           /*&& raycastHit2D[index].collider != null  && raycastHit2D[0].collider.CompareTag("Player")*/) //We are seeing the player
        {
            //Debug.Log(raycastHit2D.collider.name);
            //isSeeingPlayer = true;
            isPatrolling = false;
            locomotion.Attack();
            //Debug.DrawRay(transform.position, direction * raycastHit2D[0].distance, Color.red);
        }
        else //We are not seeing the player
        {
            isPatrolling = true;
            isSeeingPlayer = false;
            locomotion.ResetAttack();
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
        else if (isSeeingPlayer && shockRange.personInRange)
        {
            //if(locomotion.IsWindingUp() && locomotion.IsCooldownFinished())
            //{
            //    //Debug.Log("Not moving during wind up");
            //    locomotion.Move(0);
            //}
            //else
            //{
            //Debug.Log("Moving towards player");
            float dis = Vector2.Distance(shockRange.personInRange.transform.position, transform.position);
            if (dis > stoppingDistance)
            {
                locomotion.Move(direction.x);
            }
            //}
        }
    }



    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
}
