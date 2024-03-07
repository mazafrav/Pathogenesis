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

    [SerializeField]
    private LayerMask rayLayerMask;

    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    [SerializeField]
    private Color colorWhileWindUp;

    private GameObject player = null;

    private Vector2 direction = Vector2.zero;
    private bool isFinishedWindUp = false, isSeeingPlayer = false, isPatrolling = true;
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
            raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player"))
        {
            isSeeingPlayer = true;
            isPatrolling = false;
            locomotion.IsSeeingPlayer = true;
            locomotion.Attack();            
            //Debug.Log("Seeing player");
            Debug.DrawRay(transform.position, direction * raycastHit2D.distance, Color.red);
        }
        else
        {
            isPatrolling = true;
            locomotion.IsSeeingPlayer = false;
            //locomotion.ResetAttack();
            //Debug.Log("Not seeing player");
        }
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            spriteRenderers[0].color = colorWhenMoving;
            spriteRenderers[1].color = colorWhenMoving;
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
           
            if (!locomotion.IsWindingUp())
            {
                spriteRenderers[0].color = colorWhenMoving;
                spriteRenderers[1].color = colorWhenMoving;
                Debug.Log("Moving towards player");
                float dis = Vector2.Distance(player.transform.position, transform.position);
                //Debug.Log(dis);
                if (dis > stoppingDistance)
                {
                    locomotion.Move(direction.x);
                    // rb2D.velocity = new Vector2(direction.x * speed, 0.0f);
                }
            }
            else
            {
                Debug.Log("Not moving during wind up");
                locomotion.Move(0);
                spriteRenderers[0].color = colorWhileWindUp;
                spriteRenderers[1].color = colorWhileWindUp;
            }           
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }
}
