using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemy : Enemy
{
    [SerializeField]
    private float movementTime = 2.0f;

    [Header("Movement speed")]
    [SerializeField]
    private float speed = 5.0f;

    [Header("Jump")]
    [SerializeField]
    private float height = 2;
    [SerializeField]
    private float distance = 3;

    [Header("Attack")]
    [SerializeField]
    GameObject electricShockGameObject = null;

    [SerializeField]
    float cooldown = 1.0f;

    [SerializeField]
    float range = 3.0f;

    private Rigidbody2D rb2D = null;

    private bool isActiveElectricShock = false;
    private bool isMovingRight = false;
    private float g = 1.0f, velocityY = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        g = (-2 * height * speed * speed) / (distance * distance);
        
        velocityY = (2 * height * speed) / distance;

        electricShockGameObject.transform.localScale = new Vector3 (range, range, electricShockGameObject.transform.localScale.z);

        InvokeRepeating("ApplyElectricShock", cooldown, cooldown);
        InvokeRepeating("JumpAndMove", movementTime, movementTime);
    }

    // Update is called once per frame
    void Update()
    {
    }


    void JumpAndMove()
    {
        int directionX = -1;
        if(isMovingRight)
        {
            directionX = 1;
        }
        else
        {
            directionX = -1;
        }

        rb2D.velocity = new Vector2(speed * directionX, velocityY);
        isMovingRight = !isMovingRight;
    }

    void ApplyElectricShock()
    {
        isActiveElectricShock = !isActiveElectricShock;
        electricShockGameObject.SetActive(isActiveElectricShock);
    }
}
