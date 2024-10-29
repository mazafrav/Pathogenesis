using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ElectricAttackRange : MonoBehaviour //This is only used for the AI
{
    [SerializeField]
    private ElectricFollowRange electricFollowRange;

    [SerializeField]
    private Transform electricShockPivot;

    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private ContactFilter2D filter;

    private List<Collider2D> collidedObjects = new List<Collider2D>(1);

    private HostLocomotion locomotion;
    private ElectricEnemy electricEnemy;
    private ElectricLocomotion electricLocomotion;

    // Start is called before the first frame update
    void Start()
    {
        locomotion = GetComponentInParent<HostLocomotion>();
        electricLocomotion = (ElectricLocomotion)locomotion;
        electricEnemy = GetComponentInParent<ElectricEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        interactionCollider.OverlapCollider(filter, collidedObjects);
        collidedObjects.RemoveAll(obj => !obj.gameObject.CompareTag("Player") && !obj.gameObject.CompareTag("Enemy"));//We elimante objects that are not the player or enemies
        if (!electricEnemy.CanAttackSameSpecie)
        {
            collidedObjects.RemoveAll(obj => obj.GetComponent<ElectricEnemy>()); // We eliminate Electric enemies because we dont want to attack them
        }
        collidedObjects.Remove(electricEnemy.GetComponent<Collider2D>()); //I remove myself

        if (collidedObjects.Count <= 0 || electricFollowRange.VisibleTargetsInRange().Count <= 0) //We dont have any organism, we deactivate the shock
        {
            if (electricLocomotion.inAttackRange && electricLocomotion.currentRemainingShockTime > 0.0f)
            {
                //We wait a bit to deactivate the electric shock
                electricLocomotion.currentRemainingShockTime -= Time.deltaTime;
                //Debug.Log(electricLocomotion.currentRemainingShockTime);
                if (electricLocomotion.currentRemainingShockTime <= 0.0f)
                {
                    electricLocomotion.DeactivateShock();
                    electricLocomotion.inAttackRange = false;
                }
            }
        }
        else
        {
            foreach (Collider2D obj in collidedObjects) //We activate the shock
            {
                if (electricEnemy.CanAttackSameSpecie && electricEnemy.ISeeingTarget() && obj.transform.parent != null) //Possessed electric enemy
                {
                    Attack(obj.transform);
                }
                else if (electricEnemy.ISeeingTarget() && electricFollowRange.VisibleTargetsInRange().Contains(obj.gameObject) && obj.GetComponent<ElectricEnemy>() == null) //Other enemies except electric
                {
                    Attack(obj.transform);
                }
            }
        }
    }


    void Attack(Transform target)
    {
        Vector3 targetDir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(targetDir.x, targetDir.y) * Mathf.Rad2Deg;
        electricShockPivot.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        electricLocomotion.currentRemainingShockTime = electricLocomotion.ShockRemainingTime();
        locomotion.Attack();
    }
}
