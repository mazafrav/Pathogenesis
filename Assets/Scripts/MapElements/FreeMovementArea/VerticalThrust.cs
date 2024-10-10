using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VerticalThrust : MonoBehaviour
{
    [SerializeField] private float thrust = 20.0f;
    [SerializeField] private GameObject thrustBlocking;

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateThrustBlocking(false);

        PlayerLocomotion playerLocomotion = GameManager.Instance.GetPlayerLocomotion();

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>(); 

        if(playerLocomotion && rb)
        {
            playerLocomotion.DisableFreeMovement();
            rb.AddForce(new Vector3(0, 1) * thrust, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ActivateThrustBlocking(true);       
    }

    private void ActivateThrustBlocking(bool isActive)
    {
        thrustBlocking.SetActive(isActive);
        thrustBlocking.GetComponent<BoxCollider2D>().enabled = isActive;
        thrustBlocking.GetComponent<ThrustBlocking>().enabled = isActive;        
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + transform.up * 2.0f);

    }
}
