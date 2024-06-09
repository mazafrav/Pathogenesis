using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private Collider2D absorptionVFXCollider;

    [SerializeField]
    private ContactFilter2D filter;

    private List<Collider2D> collidedObjects = new List<Collider2D>(1);
    private List<Collider2D> collidedObjectsVFX = new List<Collider2D>(1);
    protected bool interacted = false;
    protected bool playerInAbsorptionVFXRange = false;

    protected virtual void Update()
    {
        //The player interacts with the absorption collider
        interactionCollider.OverlapCollider(filter, collidedObjects);
        foreach (var o in collidedObjects)
        {
            if (o)
            {
                OnCollided(o.gameObject);
            }
        }

        //The player interacts with the absorption VFX collider
        int numberOfCollidedObjects = absorptionVFXCollider.OverlapCollider(filter, collidedObjectsVFX);
        if(numberOfCollidedObjects> 0)
        {
            foreach (var o in collidedObjectsVFX)
            {
                if (o)
                {
                    playerInAbsorptionVFXRange = true;
                    OnActivateAbsorptionVFX(o.gameObject);
                }
            }
        }
        else
        {
            if(playerInAbsorptionVFXRange)
            {
                OnDeactivateAbsorptionVFX();
                playerInAbsorptionVFXRange = false;
            }
        }      
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {      
       OnInteract(collidedObject);              
    }

    protected virtual void OnActivateAbsorptionVFX(GameObject collidedObject)
    {
    }

    protected virtual void OnDeactivateAbsorptionVFX()
    {
    }

    protected virtual void OnInteract(GameObject interactedObject)
    {
        if (interacted) return;
        interacted = true;
    }
}
