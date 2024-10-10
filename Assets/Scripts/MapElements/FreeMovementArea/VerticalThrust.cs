using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalThrust : MonoBehaviour
{
    [SerializeField] private float thrust = 20.0f;
    [SerializeField] private GameObject thrustBlocking;

    public bool CanThrust { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateThrustBlocking(false);

        PlayerLocomotion playerLocomotion = GameManager.Instance.GetPlayerLocomotion();

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>(); 

        if(playerLocomotion && rb)
        {
            playerLocomotion.DisableFreeMovement();
            rb.AddForce(new Vector3(0, 1) * thrust, ForceMode2D.Impulse);
        }



        CanThrust = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ActivateThrustBlocking(true);

        Invoke("DeactivateCanThrust", 0.5f);
    }

    private void ActivateThrustBlocking(bool isActive)
    {
        thrustBlocking.SetActive(isActive);
        thrustBlocking.GetComponent<BoxCollider2D>().enabled = isActive;
        thrustBlocking.GetComponent<ThrustBlocking>().enabled = isActive;        
    }

    void DeactivateCanThrust()
    {
        CanThrust = false;
    }

    //private void ApplyThrust(PlayerController playerController, Collider2D collision)
    //{

    //    Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
    //    if (dir.x > 0.0f || dir.x < 0.0f)
    //    {
    //        playerController.enabled = false;
    //        hasDisabledControls = true;
    //    }
    //    //collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * 25, ForceMode2D.Impulse);

    //    collision.GetComponent<Rigidbody2D>().velocity = new Vector2(30, 0);


    //    //else if (applyThrustOverTheFloor)
    //    //{
    //    //    Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
    //    //    if (dir.x > 0.0f || dir.x < 0.0f)
    //    //    {
    //    //        playerController.enabled = false;
    //    //        hasDisabledControls = true;
    //    //    }
    //    //    collision.GetComponent<Rigidbody2D>().AddForce(dir * thrust, ForceMode2D.Impulse);
    //    //}
    //}
}
