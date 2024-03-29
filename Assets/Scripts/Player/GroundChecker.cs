using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded { get; private set; }

    private void Awake()
    {
        int LayerGroundChecker = LayerMask.NameToLayer("GroundChecker");
        gameObject.layer = LayerGroundChecker;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /**
        if (collision.gameObject.tag == "TileMap")
        {
            isGrounded = true;
        }
        **/
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /**
        if (collision.gameObject.tag == "TileMap")
        {
            isGrounded = false;
        }
        **/
        isGrounded = false;
    }
}
