using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HostAbsorption : Interactable
{
    [SerializeField]
    private LayerMask layerMask;
    private HostLocomotion hostLocomotion;
    private PlayerController playerController;
    private Enemy enemyBehaviour;
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        hostLocomotion = GetComponent<HostLocomotion>();
        playerController = GameManager.Instance.GetPlayerController();
        enemyBehaviour = GetComponent<Enemy>();
        Physics2D.queriesStartInColliders = false;
        Debug.Log(layerMask);      
        virtualCamera = GameManager.Instance.GetCamera();
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (collidedObject.transform.position - transform.position).normalized, 10f, ~layerMask);
        Debug.DrawLine(transform.position, collidedObject.transform.position + (collidedObject.transform.position - transform.position).normalized * 0.2f, Color.green);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    OnInteract(collidedObject);
                    playerInRange = false;
                }
            }
        }
    }

    protected override void OnInteract(GameObject interactedObject)
    {
        base.OnInteract(interactedObject);

        if (playerController.GetPlayerBody()) //we possess if the player exists
        {
            hostLocomotion.ResetAttack();
            playerController.locomotion = hostLocomotion;
            gameObject.transform.parent = playerController.transform;
            playerController.DisablePlayerBody();
            virtualCamera.Follow = hostLocomotion.transform;
            RangedEnemy rangedEnemy = GetComponent<RangedEnemy>();
            if (rangedEnemy != null)
            {
                playerController.shootingComponent = rangedEnemy.shootingComponent;
                rangedEnemy.ResetRigidbodyConstraints();
                rangedEnemy.SetAimBehaviour(true);
            }

            enemyBehaviour.enabled = false;
        }

    }

}
