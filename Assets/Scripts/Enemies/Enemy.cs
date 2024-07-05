using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected HostLocomotion locomotion;
    [Header("Movement")]
    [SerializeField]
    protected Transform[] wayPoints;
    [SerializeField]
    protected float chaseSpeed = 3.0f;
    [SerializeField]
    protected float patrolSpeed = 3.0f;


    [SerializeField]
    private float minDistanceToWaypoint = 0.3f;
    [Header("VFX")]
    [SerializeField]
    private ParticleSystem deathEffect;

    protected float movementDirection = 0;

    private int currentWayPointIndex = 0;

    public void DestroyEnemy()
    {
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

   virtual protected void Patrol()
   {
        locomotion.SetMoveSpeed(patrolSpeed);
        Vector2 dirToWaypoint = (wayPoints[currentWayPointIndex].position - transform.position).normalized;
        movementDirection = dirToWaypoint.x;
        float distanceToWayPoint = (transform.position - wayPoints[currentWayPointIndex].position).magnitude;
        locomotion.Move(dirToWaypoint.x, dirToWaypoint.y);

        if (distanceToWayPoint < minDistanceToWaypoint)
        {
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
        }
   }

}
