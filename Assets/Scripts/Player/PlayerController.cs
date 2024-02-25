using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float speed = 5.0f;

    [Header("Jump")]
    [SerializeField]
    private float height = 2;
    [SerializeField]
    private float distance = 3;

    public GroundChecker groundChecker = null;
    public Rigidbody2D rb2D = null;

    private float deltaX = 0.0f, deltaY = 0.0f;
    private float g = 1.0f, velocityY = 1.0f;

    private bool pressedJumpButton = false;
    private float jumpOffset = 0.5f;

    private bool hasFreeMovement = false;
    private float previousGravityscale = 1.0f;

    void Start()
    {
        distance += jumpOffset;
        height += jumpOffset;

        rb2D = GetComponent<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();

        if(hasFreeMovement)
        {
            rb2D.gravityScale = 0.0f;
        }
        else
        {
            g = (-2 * height * speed * speed) / ((distance / 2) * (distance / 2));
            rb2D.gravityScale = g / Physics2D.gravity.y;
            velocityY = (2 * height * speed) / (distance / 2);
        }
    }

    void Update()
    {
        deltaX = Input.GetAxisRaw("Horizontal");
        deltaY = Input.GetAxisRaw("Vertical");
        if (groundChecker.isGrounded && Input.GetButtonDown("Jump") && !hasFreeMovement)
        {
            g = (-2 * height * speed * speed) / ((distance/2.0f) * (distance/2.0f));
            rb2D.gravityScale = g / Physics2D.gravity.y;
            velocityY = (2 * height * speed) / (distance/2.0f);
            pressedJumpButton = true;
        }
    }

    private void FixedUpdate()
    {
        if (pressedJumpButton)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, velocityY);
            pressedJumpButton = false;
        } 
      
        if(hasFreeMovement)
        {
            rb2D.velocity = new Vector2(deltaX * speed, deltaY * speed);
        }
        else
        {
            rb2D.velocity = new Vector2(deltaX * speed, rb2D.velocity.y);
        }
    }

    public void EnableFreeMovement()
    {
        hasFreeMovement = true;
        previousGravityscale = rb2D.gravityScale;
        rb2D.gravityScale = 0.0f;
    }

    public void DisableFreeMovement()
    {
        hasFreeMovement = false;
        rb2D.gravityScale = previousGravityscale;
    }
}
