using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [SerializeField]
    private Collider2D interactionCollider;
    [SerializeField]
    private ContactFilter2D filter;
    private List<Collider2D> collidedObjects = new List<Collider2D>(1);
    protected bool interacted = false;
    protected bool playerInRange = false;

    protected virtual void Update()
    {
        interactionCollider.OverlapCollider(filter, collidedObjects);
        foreach (var o in collidedObjects)
        {
            if(o)
            {
                OnCollided(o.gameObject);
            }
        }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OnInteract(collidedObject);
            playerInRange = false;
        }
    }

    protected virtual void OnInteract(GameObject interactedObject)
    {
        if (interacted) return;
        interacted = true;
        Debug.Log("Interacted with " + interactedObject.name);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
