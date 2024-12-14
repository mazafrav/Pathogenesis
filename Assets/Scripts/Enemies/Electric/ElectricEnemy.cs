using System;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemy : Enemy
{
    public static event Action OnAttackSameSpecie;

    private ElectricFollowRange followRange = null;
    private ElectricLocomotion electricLocomotion = null;

    private bool isSeeingTarget = false, isPatrolling = true;

    private ShootingComponent shootingComponent;

    public Vector2 direction { get; set; } = Vector2.zero;
    public bool ISeeingTarget() { return isSeeingTarget; }
    public void SetIsSeeingTarget(bool _isSeeingTarget) { isSeeingTarget = _isSeeingTarget; }

    public ShootingComponent GetShootingComponent() { return shootingComponent; }

    protected override void Start()
    {
        base.Start();

        OnAttackSameSpecie += AllowAttackSameSpecies;

        shootingComponent = GetComponentInChildren<ShootingComponent>();
        electricLocomotion = GetComponent<ElectricLocomotion>();
        followRange = GetComponentInChildren<ElectricFollowRange>();
        Physics2D.queriesStartInColliders = false;
    }   

    void Update()
    {
        if (isSeeingTarget) //We are seeing the target
        {
            List<GameObject> visibleTargets = followRange.VisibleTargetsInRange();
            for (int i = 0; i < visibleTargets.Count; i++)
            {
                CheckIfDetected(visibleTargets[i]);
            }
            electricLocomotion.SetMoveSpeed(chaseSpeed);

            isPatrolling = false;
        }
        else
        {
            isPatrolling = true;
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
        else if (isSeeingTarget)
        {
            locomotion.Move(direction.x);
        }
    }

    public override void DestroyEnemy()
    {
        ElectricEnemy possessedEnemy = GameManager.Instance.GetPlayerController().GetComponentInParent<ElectricEnemy>();

        //If the player is possessing an electric enemy we notify the others electric enemies
        if (possessedEnemy)
        {
            possessedEnemy.transform.position += new Vector3(0.01f, 0.0f, 0.0f); //We need to move it a bit so OnTriggerStay is executed in ElectricFollowRange.cs

            OnAttackSameSpecie?.Invoke();
        }

        electricLocomotion.StopJumpLoopSFX();

        base.DestroyEnemy();
    }

    private void OnDisable()
    {
        OnAttackSameSpecie -= AllowAttackSameSpecies;
    }
}
