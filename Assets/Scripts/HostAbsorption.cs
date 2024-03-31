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
    [SerializeField]
    private SpriteRenderer graphics;
    [SerializeField]
    private SpriteRenderer weaponGraphics;
    private HostLocomotion hostLocomotion;
    private PlayerController playerController;
    private Enemy enemyBehaviour;
    public Color possessingColor { get; private set; }

    void Start()
    {
        possessingColor = new Color(0.6107601f, 0.7075472f, 0.6252144f);
        hostLocomotion = GetComponent<HostLocomotion>();
        playerController = GameManager.Instance.GetPlayerController();
        enemyBehaviour = GetComponent<Enemy>();
        Physics2D.queriesStartInColliders = false;
        Debug.Log(layerMask);
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (collidedObject.transform.position - transform.position).normalized, 10f, ~layerMask);
        Debug.DrawLine(transform.position, collidedObject.transform.position + (collidedObject.transform.position - transform.position).normalized * 0.2f, Color.green);
        bool playerLOS = false;
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerLOS = true;
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    OnInteract(collidedObject);
                    playerInRange = false;
                }
            }
        }

        if (playerLOS && playerController.AbsorbableHostInRange == null)
        {
            Debug.LogWarning(playerLOS + " : " + playerController.AbsorbableHostInRange);
            playerController.OnEnterAbsorbableRange(this);
        }
        else if (!playerLOS && playerController.AbsorbableHostInRange == this)
        {
            Debug.LogWarning("disable absorbable");
            playerController.OnLeaveAbsorbableRange();
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
            graphics.color = possessingColor;

            CinemachineVirtualCamera cinemachineVirtualCamera = GameManager.Instance.GetCamera();
            if (cinemachineVirtualCamera != null)
            {
                cinemachineVirtualCamera.Follow = hostLocomotion.transform;
            }

            RangedEnemy rangedEnemy = GetComponent<RangedEnemy>();
            if (rangedEnemy != null)
            {
                weaponGraphics.color = possessingColor;
                playerController.shootingComponent = rangedEnemy.shootingComponent;
                rangedEnemy.ResetRigidbodyConstraints();
                rangedEnemy.SetAimBehaviour(true);
            }

            enemyBehaviour.enabled = false;
        }

    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (playerController.AbsorbableHostInRange == this)
        {
            playerController.OnLeaveAbsorbableRange();
        }
    }

}
