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

    public ShootingComponent shootingComponent;
    //public float DeltaX { set { deltaX = value; } }


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
            if (!locomotion.GetType().Name.Equals("RangedLocomotion"))
            {
                locomotion.Attack();
            }
            else
            {
                locomotion.Attack(shootingComponent.mousePosition);
            }
        }
        else if(Input.GetKeyDown(KeyCode.F))
        {           
            locomotion.Unpossess();
        }
    }

    private void FixedUpdate()
    {
        locomotion.Move(deltaX, deltaY);
    }

    public void DisablePlayerBody()
    {
        if(playerBody)
        {
            playerBody.SetActive(false);
        }
    }
    public void EnablePlayerBody()
    {
        if (playerBody)
        {
            playerBody.SetActive(true);
        }

    }

    public GameObject GetPlayerBody() { return playerBody; }
}
