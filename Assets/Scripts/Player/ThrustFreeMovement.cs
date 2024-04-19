using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustFreeMovement : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
       playerController = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerController && playerController.HasDisabledControls && collision.collider.gameObject.CompareTag("TileMap"))
        {
            playerController.enabled = true;
            playerController.HasDisabledControls = false;
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (playerController && playerController.HasDisabledControls && collision.collider.gameObject.CompareTag("TileMap"))
    //    {
    //        playerController.enabled = true;
    //        playerController.HasDisabledControls = false;
    //    }
    //}
}
