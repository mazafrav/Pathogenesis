using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundChecker : MonoBehaviour
{
    [SerializeField]
    public float groundCheckDistance = 0.6f;
    public bool CanJump { get; private set; }

    public bool isGrounded()
    {
        // WORK IN PROGRESS
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.parent.transform.position, Vector2.down, groundCheckDistance);
        if (raycastHit.collider.gameObject.tag == "TileMap")
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CanJump = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CanJump = false;
    }
}
