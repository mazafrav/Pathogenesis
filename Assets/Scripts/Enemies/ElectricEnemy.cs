using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ElectricEnemy : Enemy
{
   

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
    private GameObject electricShockGameObject = null;

    [SerializeField]
    float rayRange = 1.0f;

    [SerializeField]
    private float cooldown = 1.0f;

    [SerializeField]
    private float electricShockRange = 3.0f;

    [SerializeField]
    private LayerMask playerLayerMask;

    private GameObject player = null;

    private Rigidbody2D rb2D = null;

    private bool isActiveElectricShock = true;
    private float g = 1.0f, velocityY = 1.0f, jumpOffset = 0.5f;
    private float currentCooldownTime = 0.0f, currentWindUpTime = 0.5f;

    private Vector2 direction = Vector2.zero;
    private bool isFinishedWindUp = false, isSeeingPlayer = false, isPatrolling = true;

    void Start()
    {
        player = GameObject.Find("Player");
        distance += jumpOffset;
        height += jumpOffset;
        rb2D = GetComponentInParent<Rigidbody2D>();

        g = (-2 * height * speed * speed) / ((distance / 2.0f) * (distance / 2.0f));
        rb2D.gravityScale = g / Physics2D.gravity.y;
        velocityY = (2 * height * speed) / (distance / 2.0f);
       
        electricShockGameObject.SetActive(false);
        electricShockGameObject.transform.localScale = new Vector3 (electricShockRange * 2.0f, electricShockRange * 2.0f, electricShockGameObject.transform.localScale.z);      
    }

    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, 200, playerLayerMask);
        if(Vector2.Distance(player.transform.position, transform.position) <= electricShockRange &&
            raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player"))
        {
            isSeeingPlayer = true;
            isPatrolling = false;
            Debug.Log("Seeing player");
            Debug.DrawRay(transform.position, direction * raycastHit2D.distance, Color.red);

            currentWindUpTime += Time.deltaTime;
            if(currentWindUpTime>=windUp)
            {
                isFinishedWindUp = true;
                electricShockGameObject.SetActive(isActiveElectricShock);
                currentCooldownTime += Time.deltaTime;
                if(currentCooldownTime>=cooldown)
                {
                    isActiveElectricShock = !isActiveElectricShock;
                    currentWindUpTime = 0;
                    currentCooldownTime = 0.0f;
                    isFinishedWindUp = false;
                }           
            }
        }
        else
        {
            isFinishedWindUp = false;
            isSeeingPlayer = false;
            isPatrolling = true;
            isActiveElectricShock = true;
            electricShockGameObject.SetActive(false);
            currentCooldownTime = 0.0f;
            currentWindUpTime = 0;
            Debug.Log("Not seeing player");
        }  
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            if (IsFacingRight())
            {
                rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
            }
            else
            {
                rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
            }
        }
        else
        {
            if (isSeeingPlayer && isFinishedWindUp)
            {
                float dis = Vector2.Distance(player.transform.position, transform.position);
                if (dis > stoppingDistance)
                {
                    rb2D.velocity = new Vector2(direction.x * speed, 0.0f);
                }
            }
            else
            {
                Debug.Log("Not moving during wind up");
            }
        }
    }
   
    void Jump(int jumpDirectionX)
    {
        rb2D.velocity = new Vector2(speed * jumpDirectionX, velocityY);    
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
}
