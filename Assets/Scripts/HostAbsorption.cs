using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostAbsorption : Interactable
{
    private HostLocomotion hostLocomotion;
    private PlayerController playerController;
    private Enemy enemyBehaviour;

    void Start()
    {
        hostLocomotion = GetComponent<HostLocomotion>();
        playerController = GameManager.Instance.GetPlayerController();
        enemyBehaviour = GetComponent<ElectricEnemy>();
    }
    protected override void OnInteract(GameObject interactedObject)
    {
        base.OnInteract(interactedObject);
        playerController.locomotion = hostLocomotion;
        gameObject.transform.parent = playerController.transform;
        playerController.DisablePlayerBody();
        enemyBehaviour.enabled = false;
    }

}
