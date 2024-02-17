using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Header("Player speed")]
    private float speed = 5.0f;

    [SerializeField]
    [Header("Player jump")]
    private float height = 2;
    [SerializeField]
    private float distance = 3;

    private GroundChecker groundChecker = null;

    private Rigidbody2D rb2D = null;
    private float deltaX = 0.0f;
    private bool pressedJumpButton = false;
    private float g, velocidadY;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        //g = (-2 * height * speed * speed) / ((distance / 2) * (distance / 2));   
        //rb2D.gravityScale = g / Physics2D.gravity.y;
        //velocidadY = (2 * height * speed) / (distance / 2);
    }

    // Update is called once per frame
    void Update()
    {
        deltaX = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            g = (-2 * height * speed * speed) / (distance * distance);
            rb2D.gravityScale = g / Physics2D.gravity.y;
            velocidadY = (2 * height * speed) / distance;
            pressedJumpButton = true;
        }
    }

    private void FixedUpdate()
    {
        if (pressedJumpButton && groundChecker.CanJump)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, velocidadY);
            pressedJumpButton = false;
        } 
      
        rb2D.velocity = new Vector2(deltaX * speed, rb2D.velocity.y);
    }
}
