using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ElectricEnemy : Enemy
{
    [SerializeField]
    private float movementTime = 2.0f;

    [Header("Movement")]
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float stoppingDistance = 10.0f;

    [Header("Jump")]
    [SerializeField]
    private float height = 2;
    [SerializeField]
    private float distance = 3;

    [Header("Attack")]
    [SerializeField]
    private float windUp = 0.5f;

    [SerializeField]
    GameObject electricShockGameObject = null;

    [SerializeField]
    private float cooldown = 1.0f;

    [SerializeField]
    private float electricShockRange = 3.0f;

    [SerializeField]
    private LayerMask playerLayerMask;


    [SerializeField]
    private float floatingSpeed = 3.0f;

    [SerializeField]
    private float floatingHeight = 3.0f;

    private GameObject player = null;

    private Rigidbody2D rb2D = null;

    private bool isActiveElectricShock = true;
    private bool isMovingRight = false;
    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    private float currentTime = 0.0f, currentWindUpTime = 0.5f;

    private Vector2 direction = Vector2.zero;
    private bool isFinishedWindUp = false, isSeeingPlayer = false;

    void Start()
    {
        player = GameObject.Find("Player");
        distance += jumpOffset;
        height += jumpOffset;
        rb2D = GetComponent<Rigidbody2D>();

        g = (-2 * height * speed * speed) / ((distance / 2.0f) * (distance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * height * speed) / (distance / 2.0f);

        electricShockGameObject.SetActive(false);
        electricShockGameObject.transform.localScale = new Vector3 (electricShockRange * 2.0f, electricShockRange * 2.0f, electricShockGameObject.transform.localScale.z);

        //InvokeRepeating("JumpAndMove", movementTime, movementTime);
    }

    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, 200, playerLayerMask);
        if(Vector2.Distance(player.transform.position, transform.position) <= electricShockRange &&
            raycastHit2D.collider != null && raycastHit2D.collider.tag == "Player")
        {
            isSeeingPlayer = true;
            Debug.Log("Seeing player");
            Debug.DrawRay(transform.position, direction * raycastHit2D.distance, Color.red);


            currentWindUpTime += Time.deltaTime;
            if(currentWindUpTime>=windUp)
            {
                isFinishedWindUp = true;
                electricShockGameObject.SetActive(isActiveElectricShock);
                currentTime += Time.deltaTime;
                if(currentTime>=cooldown)
                {
                    isActiveElectricShock = !isActiveElectricShock;
                    currentWindUpTime = 0;
                    currentTime = 0.0f;
                    isFinishedWindUp = false;
                }           
            }

        }
        else
        {
            isFinishedWindUp = false;
            isSeeingPlayer = false;
            isActiveElectricShock = true;
            electricShockGameObject.SetActive(false);
            currentTime = 0.0f;
            currentWindUpTime = 0;
            Debug.Log("Not seeing player");
        }  
    }

    private void FixedUpdate()
    {
        if(isSeeingPlayer && isFinishedWindUp)
        {
            float dis = Vector2.Distance(player.transform.position, transform.position);
            if(dis > stoppingDistance)
            {
                 rb2D.velocity = direction * speed;
            }
        }
        else
        {
            Debug.Log("Not moving");
        }
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
}
