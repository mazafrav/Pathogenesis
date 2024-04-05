using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
   
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
        }
    }
}
