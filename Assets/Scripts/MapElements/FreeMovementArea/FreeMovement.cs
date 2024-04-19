using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    [Header("Bounciness avoidance")]
    [SerializeField] private bool canBlockBounciness = false;
    [SerializeField] private GameObject blockingBounciness;
    [Header("Thrust")]
    [SerializeField] private bool applyThrust = false;
    [SerializeField] private bool applyThrustOverTheFloor = false;
    [SerializeField] private float thrust = 20.0f;

    [SerializeField] private float time = 1.0f;
    private bool hasDisabledControls = false;
    private float currentTime = 0.0f;

    private void Start()
    {
        currentTime = time;
        ActivateBlockingBounciness(false);
    }

    private void Update()
    {
        if (hasDisabledControls)
        {
            currentTime -= Time.deltaTime;
        }
        if (currentTime <= 0.0f)
        {
            currentTime = time;
            hasDisabledControls = false;
            PlayerController playerController = GameManager.Instance.GetPlayerController();
            playerController.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();
        if (playerLocomotion && playerController && playerController.GetPlayerBody().gameObject.activeSelf)
        {
            playerLocomotion.EnableFreeMovement();

            if (canBlockBounciness)
            {
                ActivateBlockingBounciness(false);
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
            
            //We block the bouncines that happen when the player goes from bottom to up
            if (canBlockBounciness && playerController.GetDeltaY() > 0.0f)
            {
                ActivateBlockingBounciness(true);
            }         
        }
    }


    private void ActivateBlockingBounciness(bool isActive)
    {
        blockingBounciness.SetActive(isActive);
        blockingBounciness.GetComponent<BoxCollider2D>().enabled = isActive;
        blockingBounciness.GetComponent<BlockingBouncines>().enabled = isActive;
    }

    private void ApplyThrust(PlayerController playerController, Collider2D collision)
    {
        if (applyThrust)
        {
            Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
            if (dir.x > 0.0f || dir.x < 0.0f)
            {
                playerController.enabled = false;
                playerController.HasDisabledControls = true;
            }
            collision.GetComponent<Rigidbody2D>().AddForce(dir * thrust, ForceMode2D.Impulse);
        }
        else if (applyThrustOverTheFloor)
        {
            Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
            if (dir.x > 0.0f || dir.x < 0.0f)
            {
                playerController.enabled = false;
                hasDisabledControls = true;
            }
            collision.GetComponent<Rigidbody2D>().AddForce(dir * thrust, ForceMode2D.Impulse);
        }
    }
}
