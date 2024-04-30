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

    [SerializeField]
    private float cameraShakeIntensity = 0.6f;
    [SerializeField]
    private float cameraShakeTime = 0.3f;


    private HostLocomotion hostLocomotion;
    private PlayerController playerController;
    private PlayerLocomotion playerLocomotion;
    private Enemy enemyBehaviour;
    public Color possessingColor { get; private set; }

    void Start()
    {
        possessingColor = new Color(0.6696f, 0.7624f, 0.7981f);
        hostLocomotion = GetComponent<HostLocomotion>();
        playerController = GameManager.Instance.GetPlayerController();
        playerLocomotion = GameManager.Instance.GetPlayerLocomotion();
        enemyBehaviour = GetComponent<Enemy>();
        Physics2D.queriesStartInColliders = false;
        Debug.Log(layerMask);
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (collidedObject.transform.position - transform.position).normalized, 10f, ~layerMask);
        Debug.DrawLine(transform.position, collidedObject.transform.position + (collidedObject.transform.position - transform.position).normalized * 0.2f, Color.green);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {                          
                OnInteract(collidedObject);
            }
        }      
    }

    protected override void OnActivateAbsorptionVFX(GameObject collidedObject)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (collidedObject.transform.position - transform.position).normalized, 10f, ~layerMask);
        Debug.DrawLine(transform.position, collidedObject.transform.position + (collidedObject.transform.position - transform.position).normalized * 0.2f, Color.green);
        /*
        bool playerLOS = false;
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {                              
                playerLOS = true;               
            }
        }
        */
        if (/*playerLOS &&*/ playerController.AbsorbableHostInRange == null)
        {
            // Debug.LogWarning(playerLOS + " : " + playerController.AbsorbableHostInRange);
            playerController.OnEnterAbsorbableRange(this);
        }
        else if (/*!playerLOS &&*/ playerController.AbsorbableHostInRange == this)
        {
            Debug.LogWarning("disable absorbable");
            playerController.OnLeaveAbsorbableRange();
        }
    }

    protected override void OnDeactivateAbsorptionVFX()
    {
        playerController.OnLeaveAbsorbableRange();
    }

    protected override void OnInteract(GameObject interactedObject)
    {
        base.OnInteract(interactedObject);

        if (playerController.GetPlayerBody()) //we possess if the player exists
        {
            playerLocomotion.DisableFreeMovement();
            hostLocomotion.ResetAttack();
            hostLocomotion.SetPossessingParameters();
            playerController.locomotion = hostLocomotion;
            gameObject.transform.parent = playerController.transform;
            playerController.DisablePlayerBody();
            graphics.color = possessingColor;

            CinemachineVirtualCamera cinemachineVirtualCamera = GameManager.Instance.GetCamera();
            if (cinemachineVirtualCamera != null)
            {
                cinemachineVirtualCamera.Follow = hostLocomotion.transform;
            }

            CameraShake cameraShake = cinemachineVirtualCamera.GetComponent<CameraShake>();
            if(cameraShake)
            {
                cameraShake.ShakeCamera(cameraShakeTime, cameraShakeTime);
            }
           

            RangedEnemy rangedEnemy = GetComponent<RangedEnemy>();
            if (rangedEnemy != null)
            {
                weaponGraphics.color = possessingColor;
                playerController.shootingComponent = rangedEnemy.shootingComponent;
                rangedEnemy.GetComponent<LineRenderer>().enabled = false;
                rangedEnemy.ResetRigidbodyConstraints();
                rangedEnemy.SetAimBehaviour(true);
            }

            enemyBehaviour.enabled = false;
        }

    }
}
