using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustBlocking : MonoBehaviour
{
    [SerializeField] private float blockingThrust = 1.5f;
    [SerializeField] private Transform thrustTrigger;
    [SerializeField] private Transform blockingBouncines;

    private bool isOnThrustBlocking = false;
    private Collider2D colliderThrustTrigger;
    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        colliderThrustTrigger = thrustTrigger.GetComponent<Collider2D>();
        playerRigidbody = GameManager.Instance.GetPlayerController().GetComponentInChildren<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isOnThrustBlocking)
        {
            playerRigidbody.position += new Vector2(0, 1) * blockingThrust * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GetPlayerLocomotion().EnableFreeMovement();
        colliderThrustTrigger.isTrigger = false;
        isOnThrustBlocking = true;

        blockingBouncines.gameObject.SetActive(false);
        blockingBouncines.GetComponent<BoxCollider2D>().enabled = false;
        blockingBouncines.GetComponent<BlockingBouncines>().enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.GetPlayerLocomotion().DisableFreeMovement();

        colliderThrustTrigger.isTrigger = true;
        isOnThrustBlocking = false;
        blockingBouncines.gameObject.SetActive(true);
        blockingBouncines.GetComponent<BoxCollider2D>().enabled = true;
        blockingBouncines.GetComponent<BlockingBouncines>().enabled = true;

    }
}
