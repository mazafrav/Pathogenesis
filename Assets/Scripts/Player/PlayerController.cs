using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement speed")]
    [SerializeField]
    private float speed = 5.0f;

    [Header("Jump")]
    [SerializeField]
    private float height = 2;
    [SerializeField]
    private float distance = 3;

    public GroundChecker groundChecker = null;

    public Rigidbody2D rb2D = null;
    private float deltaX = 0.0f;
    private bool pressedJumpButton = false;
    private float g = 1.0f, velocityY = 1.0f;
    private float jumpOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        distance += jumpOffset;
        height += jumpOffset;
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
        if (groundChecker.CanJump && Input.GetButtonDown("Jump"))
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
      
        rb2D.velocity = new Vector2(deltaX * speed, rb2D.velocity.y);
    }
}
