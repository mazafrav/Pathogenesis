using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttackRange : MonoBehaviour
{
    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private ContactFilter2D filter;

    private List<Collider2D> collidedObjects = new List<Collider2D>(1);

    private HostLocomotion locomotion;
    private ElectricEnemy electricEnemy;

    // Start is called before the first frame update
    void Start()
    {
        locomotion = GetComponentInParent<HostLocomotion>();
        electricEnemy = GetComponentInParent<ElectricEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        interactionCollider.OverlapCollider(filter, collidedObjects);
        collidedObjects.RemoveAll(obj => !obj.gameObject.CompareTag("Player") && !obj.gameObject.CompareTag("Enemy"));//We elimante objects that are not the player or enemies
        collidedObjects.RemoveAt(0); // We eliminate the Electric enemy

        if(collidedObjects.Count <= 0) //We dont have any organism, we deactivate the shock
        {
            ElectricLocomotion d = (ElectricLocomotion)locomotion;
            d.DeactivateShock();
        }
        else
        {
            foreach (Collider2D obj in collidedObjects) //We activate the shock
            {
                if (electricEnemy.ISeeingTarget() && (obj.gameObject.CompareTag("Player") || obj.gameObject.CompareTag("Enemy")))
                {
                    locomotion.Attack();
                }           
            }
        }
    }
}
