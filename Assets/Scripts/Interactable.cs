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

    protected virtual void Update()
    {
        interactionCollider.OverlapCollider(filter, collidedObjects);
        foreach (var o in collidedObjects)
        {
            OnCollided(o.gameObject);
        }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract(collidedObject);
        }
    }

    protected virtual void OnInteract(GameObject interactedObject)
    {
        if (interacted) return;
        interacted = true;
        Debug.Log("Interacted with " + interactedObject.name);
    }
}
