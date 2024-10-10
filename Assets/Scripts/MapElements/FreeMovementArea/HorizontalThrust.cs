using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalThrust : MonoBehaviour
{
    [SerializeField] private float thrust = 30.0f;
    [SerializeField] private float time = 1.0f;
    private bool hasDisabledControls = false;
    private float currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = time;
    }

    // Update is called once per frame
    void Update()
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

        Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
        if (dir.x > 0.0f || dir.x < 0.0f)
        {
            playerController.enabled = false;
            hasDisabledControls = true;
        }
        playerLocomotion.EnableFreeMovement();
        collision.GetComponent<Rigidbody2D>().velocity = new Vector2(thrust, 0);
    }

    
}
