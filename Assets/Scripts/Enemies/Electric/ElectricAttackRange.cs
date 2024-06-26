using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttackRange : MonoBehaviour
{

    HostLocomotion locomotion;
    ElectricEnemy electricEnemy;

    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private ContactFilter2D filter;

    private List<Collider2D> collidedObjects = new List<Collider2D>(1);

    public bool isPlayerInAttackRange { get; private set; }

    public bool IsSeeingTraget() { return electricEnemy.ISeeingTarget(); }

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
        collidedObjects.RemoveAll(obj => !obj.gameObject.CompareTag("Player") && !obj.gameObject.CompareTag("Enemy"));
        collidedObjects.RemoveAt(0);

        if(collidedObjects.Count <= 0)
        {
            ElectricLocomotion d = (ElectricLocomotion)locomotion;
            d.DeactivateShock();
        }
        else
        {

            foreach (Collider2D obj in collidedObjects)
            {
                if (collidedObjects.Count > 0 && electricEnemy.ISeeingTarget() && (obj.gameObject.CompareTag("Player") || obj.gameObject.CompareTag("Enemy")))
                {
                    locomotion.Attack();
                }           
            }
        }

    }
}
