using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected HostLocomotion locomotion;
    [SerializeField]
    protected Transform[] wayPoints;
    [SerializeField]
    private ParticleSystem deathEffect;

    protected float movementDirection = 0;

    private int currentWayPointIndex = 0;
    private float minDistanceToWaypoint = 0.3f;

    public void DestroyEnemy()
    {
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

   virtual protected void Patrol()
   {
        Vector2 dirToWaypoint = (wayPoints[currentWayPointIndex].position - transform.position).normalized;
        movementDirection = dirToWaypoint.x;
        float distanceToWayPoint = (transform.position - wayPoints[currentWayPointIndex].position).magnitude;
        locomotion.Move(dirToWaypoint.x);

        if (distanceToWayPoint < minDistanceToWaypoint)
        {
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length;
        }
   }

}
