using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    [Header("Bounciness avoidance")]
    [SerializeField] private bool canBlockBounciness = false;
    [SerializeField] private GameObject[] blockingBounciness;
    [Header("Thrust")]
    [SerializeField] private bool applyThrust = false;
    [SerializeField] private GameObject[] thrustBlockings;
    [SerializeField] private bool applyThrustOverTheFloor = false;
    [SerializeField] private float thrust = 20.0f;
    [SerializeField] private float time = 1.0f;
    [Header("Player speed modifier")]
    [SerializeField] float speedModifier = 0.8f;

    private bool hasDisabledControls = false;
    private float currentTime = 0.0f;

    private void Start()
    {
        currentTime = time;
        ActivateBlockingBounciness(false);
        ActivateThrustBlocking(applyThrust);
    }

    private void Update()
    {
        //if (hasDisabledControls)
        //{
        //    currentTime -= Time.deltaTime;
        //}
        //if (currentTime <= 0.0f)
        //{
        //    currentTime = time;
        //    hasDisabledControls = false;
        //    PlayerController playerController = GameManager.Instance.GetPlayerController();
        //    playerController.enabled = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();
        if (playerLocomotion && playerController && playerController.GetPlayerBody().gameObject.activeSelf)
        {
            playerLocomotion.EnableFreeMovement(speedModifier);

            if (canBlockBounciness)
            {
                ActivateBlockingBounciness(false);
            }

            if (applyThrust)
            {
                ActivateThrustBlocking(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();
        if (playerLocomotion && playerController && playerController.GetPlayerBody().gameObject.activeSelf)
        {
            playerLocomotion.DisableFreeMovement();

            ApplyThrust(playerController, collision);
            Debug.Log("fuerza");
            //We block the bouncines that happen when the player goes from bottom to up
            if (canBlockBounciness && playerController.GetDeltaY() > 0.0f)
            {
                ActivateBlockingBounciness(true);
            }

            if (applyThrust && playerController.GetDeltaY() > 0.0f)
            {
                ActivateThrustBlocking(true);
            }
        }
    }


    private void ActivateBlockingBounciness(bool isActive)
    {
        foreach (GameObject obj in blockingBounciness)
        {
            obj.SetActive(isActive);
            obj.GetComponent<BoxCollider2D>().enabled = isActive;
            obj.GetComponent<BlockingBouncines>().enabled = isActive;
        }
    }

    private void ActivateThrustBlocking(bool isActive)
    {
        foreach (GameObject obj in thrustBlockings)
        {
            obj.SetActive(isActive);
            obj.GetComponent<BoxCollider2D>().enabled = isActive;
            obj.GetComponent<ThrustBlocking>().enabled = isActive;
        }
    }

    private void ApplyThrust(PlayerController playerController, Collider2D collision)
    {
        if (applyThrust && GetComponentInChildren<ThrustTrigger>().CanThrust)
        {
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(0,1) * thrust, ForceMode2D.Impulse);
        }
    }
}
