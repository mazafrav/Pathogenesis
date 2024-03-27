using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLocomotion playerController = collision.GetComponentInParent<PlayerLocomotion>();
        if (playerController != null)
        {
            playerController.EnableFreeMovement();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerLocomotion playerController = collision.GetComponentInParent<PlayerLocomotion>();
        if (playerController != null)
        {
            playerController.DisableFreeMovement();
        }
    }
}
