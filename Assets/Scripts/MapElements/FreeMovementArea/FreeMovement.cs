using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    [Header("Bounciness avoidance")]
    [SerializeField] private bool canBlockBounciness = false;
    [SerializeField] private GameObject[] blockingBounciness;
    [Header("Player speed modifier")]
    [SerializeField] float speedModifier = 0.8f;

    private void Start()
    {
        ActivateBlockingBounciness(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInChildren<PlayerController>();
        if (playerLocomotion && playerController && playerController.GetPlayerBody().gameObject.activeSelf)
        {
            playerLocomotion.EnableFreeMovement(speedModifier);

            if (canBlockBounciness)
            {
                ActivateBlockingBounciness(false);
            }          
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInChildren<PlayerController>();
        if (playerLocomotion && playerController && playerController.GetPlayerBody().gameObject.activeSelf)
        {
            playerLocomotion.DisableFreeMovement();
          
            //We block the bouncines that happen when the player goes from bottom to up
            if (canBlockBounciness && playerController.GetDeltaY() > 0.0f)
            {
                ActivateBlockingBounciness(true);
            }        
        }
    }

    private void ActivateBlockingBounciness(bool isActive)
    {
        foreach (GameObject obj in blockingBounciness)
        {
            obj.SetActive(isActive);
            obj.GetComponent<BoxCollider2D>().enabled = isActive;
            obj.GetComponent<BlockingBouncines>().enabled = isActive;
        }
    }
}
