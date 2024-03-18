using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostAbsorption : Interactable
{
    private HostLocomotion hostLocomotion;
    private PlayerController playerController;

    void Start()
    {
        hostLocomotion = GetComponent<HostLocomotion>();
        playerController = GameManager.Instance.GetPlayer();
    }
    protected override void OnInteract(GameObject interactedObject)
    {
        base.OnInteract(interactedObject);
        playerController.locomotion = hostLocomotion;
        gameObject.transform.parent = playerController.transform;
        playerController.DisablePlayerBody();
    }

}
