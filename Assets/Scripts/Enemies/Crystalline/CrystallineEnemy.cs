using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalineEnemy : Enemy
{
    public GameObject currentRangedTarget = null;
    public List<GameObject> closeTargets;

    [SerializeField] 
    private ElectricShockRange range; // we use the same behaviour as the electric enemy xd
    [SerializeField]
    private PhotonicDetection photonicRange;
    [SerializeField] 
    public GameObject graphics;

    [SerializeField] private float photonicDetectionRange = 7f;
    [SerializeField] private float detectionRange = 4f;
    [SerializeField] private float stabRange = 2f;

    public float timeToCancelAggro = 1.5f;


    private bool isSeeingTarget = false;
    private Vector3 direction;

    void Start()
    {
        closeTargets = new List<GameObject>();
        range.transform.localScale = new Vector3(detectionRange * 2.0f, detectionRange * 2.0f, range.transform.localScale.z);
        photonicRange.transform.localScale = new Vector3(photonicDetectionRange * 2.0f, photonicDetectionRange * 2.0f, 
            photonicRange.transform.localScale.z);

    }

    void Update()
    {
        if (!isSeeingTarget) // if it's not seeing any target, just patrol
        {
            UpdateTarget();
            if (currentRangedTarget != null)
            {
                UpdateOrientation(currentRangedTarget.transform.position);
            }
            else
            {

                //if (movementDirection > 0)
                //{
                //    graphics.transform.rotation = Quaternion.Euler(0, 0, -90);
                //}
                //else if (movementDirection < 0)
                //{
                //    graphics.transform.rotation = Quaternion.Euler(0, 0, 90);
                //}
            }
            
            if (range.personInRange) // if an organism enters in his detection range, we check if there are any obstacles (if it can see its target)
            {
                direction = (range.personInRange.transform.position - transform.position).normalized;

                RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, direction, detectionRange);
                for (int i = 0; i < raycastHit2D.Length; i++)
                {                                                                     //Crystalline enemies dont attack crystalline enemies
                    if (raycastHit2D[i].collider.gameObject == range.personInRange && range.personInRange.GetComponent<CrystalineEnemy>() == null)
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
            
        }
        else
        {
            if (range.personInRange)
            {
                if (Vector3.Distance(transform.position, range.personInRange.transform.position) <= stabRange)
                {
                    locomotion.Attack();
                }
                else
                {
                    locomotion.Move(direction.x, direction.y);
                }
                UpdateOrientation(range.personInRange.transform.position);
            }

            else
            {
                isSeeingTarget = false;
            }
           
        }
    }


    private void FixedUpdate()
    {
        if (!isSeeingTarget)
        {
            if (wayPoints.Length != 0)
            {
                Patrol();

            }

        }
    }

    private void UpdateTarget()
    {
        if (currentRangedTarget == null)
        {
            GameObject closestRangedTarget = null;
            float distanceToClosestRangedTarget = float.PositiveInfinity;
            GameObject closestTarget = null;
            float distanceToClosestTarget = float.PositiveInfinity;
            for (int i = closeTargets.Count - 1; i >= 0; i--)
            {
                GameObject target = closeTargets[i];
                if (target == null)
                {
                    closeTargets.RemoveAt(i);
                    continue;
                }

                float dist = Vector2.Distance(target.transform.position, transform.position);
                if (closestTarget == null || dist < distanceToClosestTarget)
                {
                    closestTarget = target;
                    distanceToClosestTarget = dist;
                }

                if (target.GetComponent<RangedLocomotion>() == null)
                {
                    continue;
                }
                if (closestRangedTarget == null || dist < distanceToClosestRangedTarget)
                {
                    closestRangedTarget = target;
                    distanceToClosestRangedTarget = dist;
                }
            }
            currentRangedTarget = closestRangedTarget;
            // currentTarget = closestTarget;
        }
    }

    private void UpdateOrientation(Vector3 position)
    {
        locomotion.Aim(position);
    }

}
