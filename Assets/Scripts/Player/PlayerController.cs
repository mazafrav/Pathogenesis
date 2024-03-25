using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private GameObject playerBody;

    [SerializeField]
    public HostLocomotion locomotion;
    private float deltaX = 0.0f, deltaY = 0.0f;

    public float DeltaX { set { deltaX = value; } }


    void Start()
    {

    }

    void Update()
    {
        deltaX = Input.GetAxisRaw("Horizontal");
        deltaY = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            locomotion.Jump(deltaX);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            locomotion.JumpButtonUp();
        }
        else if(Input.GetMouseButtonDown(0))
        {
            locomotion.Attack();
        }
    }

    private void FixedUpdate()
    {
        locomotion.Move(deltaX, deltaY);
    }

    public void DisablePlayerBody()
    {
        playerBody.SetActive(false);
    }
    public void EnablePlayerBody()
    {
        playerBody.SetActive(true);
    }
}
