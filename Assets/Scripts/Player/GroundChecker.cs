using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded { get; private set; }
    private int groundCount = 0;

    private void Awake()
    {
        int LayerGroundChecker = LayerMask.NameToLayer("GroundChecker");
        gameObject.layer = LayerGroundChecker;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TileMap" || collision.gameObject.tag == "MapElement")
        {
            groundCount++;
        }

        isGrounded = groundCount > 0;
        if (isGrounded)
        {
            Animator animator = GetComponentInParent<Animator>();
            if (animator != null) { animator.SetBool("Grounded", true); }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TileMap" || collision.gameObject.tag == "MapElement")
        {
            groundCount = Mathf.Max(groundCount - 1, 0);           
        }

        isGrounded = groundCount > 0;
        if (!isGrounded)
        {
            Animator animator = GetComponentInParent<Animator>();
            if (animator != null) { animator.SetBool("Grounded", false); }
        }
    }
}
