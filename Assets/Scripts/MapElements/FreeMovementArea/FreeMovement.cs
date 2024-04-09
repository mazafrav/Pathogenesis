using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    [SerializeField] private bool canBlockBouncines = false;
    [SerializeField] private GameObject blockingBouncines;
    [SerializeField] private bool applyThrust = false;
    [SerializeField] private float thrust = 20.0f;

    private void Start()
    {
        blockingBouncines.SetActive(false);
        blockingBouncines.GetComponent<BoxCollider2D>().enabled = false;
        blockingBouncines.GetComponent<BlockingBouncines>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();
        if (playerLocomotion && playerController && playerController.GetPlayerBody().gameObject.activeSelf)
        {
            playerLocomotion.EnableFreeMovement();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();
        if (playerLocomotion && playerController && playerController.GetPlayerBody().gameObject.activeSelf)
        {
            playerLocomotion.DisableFreeMovement();

            if (applyThrust)
            {
                Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());               
                collision.GetComponent<Rigidbody2D>().AddForce(dir * thrust, ForceMode2D.Impulse);
            }
            
            if (canBlockBouncines && playerController.GetDeltaY() > 0.0f)
            {
                blockingBouncines.SetActive(true);
                blockingBouncines.GetComponent<BoxCollider2D>().enabled = true;
                blockingBouncines.GetComponent<BlockingBouncines>().enabled = true;
            }         
        }
    }
}
