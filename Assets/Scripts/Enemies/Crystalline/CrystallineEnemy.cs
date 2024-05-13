using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalineEnemy : Enemy
{
    private GameObject currentRangedTarget = null;
    private List<GameObject> closeTargets;

    [SerializeField] 
    private ElectricShockRange range; // we use the same behaviour as the electric enemy xd
    [SerializeField] 
    public GameObject graphics;

    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float stabRange = 2f;


    private bool isSeeingTarget = false, isPatrolling = true;
    private Vector3 direction;

    void Start()
    {
        closeTargets = new List<GameObject>();
        range.transform.localScale = new Vector3(detectionRange * 2.0f, detectionRange * 2.0f, range.transform.localScale.z);
    }

    void Update()
    {
        if (!isSeeingTarget) // if it's not seeing any target, just patro
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
                {
                    if (raycastHit2D[i].collider.gameObject == range.personInRange)
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
                    locomotion.Move(direction.x);
                }
                UpdateOrientation(range.personInRange.transform.position);

                //if (Mathf.Abs(range.personInRange.transform.position.y - transform.position.y) >= 0.5f)
                //{
                //    isSeeingTarget = false;
                //}
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            closeTargets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            closeTargets.Remove(collision.gameObject);
            if (collision.gameObject == currentRangedTarget)
            {
                currentRangedTarget = null;
            }
        }
    }


}
