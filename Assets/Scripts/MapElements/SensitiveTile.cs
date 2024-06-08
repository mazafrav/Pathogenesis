using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SensitiveTile : MonoBehaviour
{
    //ONE WAY TRIGGER
    //Will only trigger if exiting in the same direction that the body entered the collision

    [SerializeField]
    public GameObject[] activatableElements;
    [SerializeField]
    public Animator animator;
    // Start is called before the first frame update
    private Vector2 velocityEnter = Vector2.zero;
    private IActivatableElement[] activatableInterfaces;

    private void Start()
    {
        /*
        for (var index = 0; index < activatableElements.Length; index++)
        {
            if (activatableElements[index].GetComponent<IActivatableElement>() == null) 
            { 
                throw new System.Exception("Object does not implement IActivaatbleElement"); 
            }
            GameObject objectActivable = activatableElements[index];

            activatableInterfaces[index] = objectActivable.GetComponent<IActivatableElement>();
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        if (other.tag == "Player")
        {
            velocityEnter = other.gameObject.GetComponentInParent<PlayerLocomotion>().rb2D.velocity.normalized;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit");
        if (other.tag == "Player")
        {
            Vector2 velocityExit = other.gameObject.GetComponentInParent<PlayerLocomotion>().rb2D.velocity.normalized;
            if (Vector2.Dot(velocityEnter, velocityExit) > 0)
            {
                ActivateActivables();
            }
        }
    }

    private void ActivateActivables()
    {
        animator.Play("SensitiveTileActivated");
        for (var index = 0; index < activatableElements.Length; index++)
        {
            activatableElements[index].GetComponent<IActivatableElement>().Activate();
        }
    }

}
