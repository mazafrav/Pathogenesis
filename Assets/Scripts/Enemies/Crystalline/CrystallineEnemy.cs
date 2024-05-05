using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalineEnemy : Enemy
{
    private GameObject currentRangedTarget = null;
    private List<GameObject> closeTargets;

    void Start()
    {
        closeTargets = new List<GameObject>();
    }

    void Update()
    {
        UpdateTarget();
        if (currentRangedTarget != null)
        {
            UpdateOrientation();
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

    private void UpdateOrientation()
    {
        locomotion.Aim(currentRangedTarget.transform.position);
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
