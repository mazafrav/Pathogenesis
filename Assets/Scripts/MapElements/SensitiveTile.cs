using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitiveTile : MonoBehaviour
{
    //ONE WAY TRIGGER
    //Will only trigger if exiting in the same direction that the body entered the collision

    [SerializeField]
    public GameObject activatableElement;
    // Start is called before the first frame update
    private Vector2 velocityEnter = Vector2.zero;
    private bool isActive = false;
    private IActivatableElement activatableInterface;

    private void Start()
    {
        activatableInterface = activatableElement.GetComponent<IActivatableElement>();
        if (activatableInterface == null) { throw new System.Exception("Object does not implement IActivaatbleElement"); }
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
                isActive = !isActive;
                if (isActive)
                {
                    activatableInterface.Activate();
                }
                else
                {
                    activatableInterface.Deactivate();
                }
                Debug.Log("triggered " + isActive);
            }
        }
    }
}
