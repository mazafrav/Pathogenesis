using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    [Header("Bounciness avoidance")]
    [SerializeField] private bool canBlockBouncines = false;
    [SerializeField] private GameObject blockingBouncines;
    [Header("Thrust")]
    [SerializeField] private bool applyThrust = false;
    [SerializeField] private float thrust = 20.0f;
    [SerializeField] private float time = 1.0f;
    private bool hasDisabledControls = false;
    private float currentTime = 0.0f;

    private void Start()
    {
        currentTime = time;
        blockingBouncines.SetActive(false);
        blockingBouncines.GetComponent<BoxCollider2D>().enabled = false;
        blockingBouncines.GetComponent<BlockingBouncines>().enabled = false;
    }

    private void Update()
    {
        if(hasDisabledControls)
        {
            currentTime -= Time.deltaTime;
        }
        if(currentTime<=0.0f)
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();
        if (playerLocomotion && playerController && playerController.GetPlayerBody().gameObject.activeSelf)
        {
            playerLocomotion.DisableFreeMovement();

            if (applyThrust)
            {
                Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());               
                collision.GetComponent<Rigidbody2D>().AddForce(dir * thrust, ForceMode2D.Impulse);

                if(dir.x > 0.0f || dir.x < 0.0f)
                {
                    playerController.enabled = false;
                    hasDisabledControls = true;
                }
            }
            
            //We block the bouncines that happen when the player goes from bottom to up
            if (canBlockBouncines && playerController.GetDeltaY() > 0.0f)
            {
                blockingBouncines.SetActive(true);
                blockingBouncines.GetComponent<BoxCollider2D>().enabled = true;
                blockingBouncines.GetComponent<BlockingBouncines>().enabled = true;
            }         
        }
    }
}
