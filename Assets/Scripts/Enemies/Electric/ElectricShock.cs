using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElectricShock : MonoBehaviour
{
    [SerializeField]
    private ElectricFollowRange followRange;
    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private ContactFilter2D filter;

    private List<Collider2D> collidedObjects = new List<Collider2D>(1);

    private DamageControl damageControl;

    private void Awake()
    {
        damageControl = GetComponentInParent<DamageControl>();
    }

    private void Update()
    {
        interactionCollider.OverlapCollider(filter, collidedObjects);

        foreach (var o in collidedObjects)
        {
            if (o.gameObject == followRange.chosenTarget)
            {             
               damageControl.Damage(o);
            }   
            else if (o.gameObject.GetComponent<ElectroReceptor>() != null)
            {
                ElectroReceptor electroReceptor = o.gameObject.GetComponent<ElectroReceptor>();
                if(!electroReceptor.isActive)
                {
                    electroReceptor.ElectroShock();
                }
            }
            else if(o.gameObject.GetComponent<CrystalBlock>() != null)
            {
                o.gameObject.GetComponent<CrystalBlock>().DestroyCrystalBlock();
            }
        }       
    }   
}
