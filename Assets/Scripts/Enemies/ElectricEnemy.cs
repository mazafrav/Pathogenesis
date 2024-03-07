using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ElectricEnemy : Enemy
{

    [Header("Movement")]
    [SerializeField]
    ElectricLocomotion locomotion;
    [SerializeField]
    private float stoppingDistance = 10.0f;

    [Header("Ray detection layer mask")]
    [SerializeField]
    private LayerMask rayLayerMask;

    [Header("WindUp feedback")]
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    [SerializeField]
    private Color colorWhileWindUp;

    private GameObject player = null;

    private Vector2 direction = Vector2.zero;
    private bool isSeeingPlayer = false, isPatrolling = true;
    private Color colorWhenMoving;

    void Start()
    {
        //Habria q hacerlo x game manager probablemente
        player = GameObject.Find("Player");

        colorWhenMoving = spriteRenderers[0].color;
    }

    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, locomotion.ElectricShockRange, rayLayerMask);
        if(Vector2.Distance(player.transform.position, transform.position) <= locomotion.ElectricShockRange &&
            raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player")) //We are seeing the player
        {
            isSeeingPlayer = true;
            isPatrolling = false;
            locomotion.IsSeeingPlayer = true;
            locomotion.Attack();            
            //Debug.Log("Seeing player");
            Debug.DrawRay(transform.position, direction * raycastHit2D.distance, Color.red);
        }
        else //We are not seeing the player
        {
            ChangeSpritesColor(colorWhenMoving);
            isPatrolling = true;
            locomotion.IsSeeingPlayer = false;
            locomotion.ResetAttack();
            //Debug.Log("Not seeing player");
        }
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            if (IsFacingRight())
            {
                locomotion.Move(1);
            }
            else
            {
                locomotion.Move(-1);
            }
        }
        else if(isSeeingPlayer)
        {                                        
            if(locomotion.IsWindingUp() && locomotion.IsCooldownFinished())
            {
                Debug.Log("Not moving during wind up");
                locomotion.Move(0);
                ChangeSpritesColor(colorWhileWindUp);
            }
            else
            {
                ChangeSpritesColor(colorWhenMoving);
                Debug.Log("Moving towards player");
                float dis = Vector2.Distance(player.transform.position, transform.position);
                //Debug.Log(dis);
                if (dis > stoppingDistance)
                {
                    locomotion.Move(direction.x);
                    // rb2D.velocity = new Vector2(direction.x * speed, 0.0f);
                }
            }
        }
    }

    private void ChangeSpritesColor(Color newColor)
    {
        spriteRenderers[0].color = newColor;
        spriteRenderers[1].color = newColor;
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
}
