using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VerticalThrust : MonoBehaviour
{
    [SerializeField] private float thrust = 20.0f;
    [SerializeField] private GameObject thrustBlocking;
    [SerializeField]private bool showArrowGizmo = true;
   
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showArrowGizmo)
        {
            Gizmos.color = Color.red;

            float lineLenght = 2.0f;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * lineLenght);

            float arrowLineLenght = 0.3f;
            Vector3 startPos = transform.position + transform.up * lineLenght;
            Gizmos.DrawLine(startPos, startPos + new Vector3(1,-1) * arrowLineLenght);    
            Gizmos.DrawLine(startPos, startPos + new Vector3(-1,-1) * arrowLineLenght);       
        }
    }
#endif
}
