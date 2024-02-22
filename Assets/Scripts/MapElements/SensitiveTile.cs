using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitiveTile : MonoBehaviour
{
    //ONE WAY TRIGGER
    //Will only trigger if exiting in the same direction that the body entered the collision

    [SerializeField]
    public SensitiveDoor door;
    // Start is called before the first frame update
    private Vector2 velocityEnter = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        if (other.tag == "Player")
        {
            velocityEnter = other.GetComponent<Rigidbody2D>().velocity.normalized;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit");
        if (other.tag == "Player")
        {
            Vector2 velocityExit = other.GetComponent<Rigidbody2D>().velocity.normalized;
            if (Vector2.Dot(velocityEnter, velocityExit) > 0)
            {
                door.activateDoor();
                Debug.Log("triggered");
            }
        }
    }
}
