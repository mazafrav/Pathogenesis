using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private PlayerLocomotion locomotion;
    private float deltaX = 0.0f, deltaY = 0.0f;


    void Start()
    {

    }

    void Update()
    {
        deltaX = Input.GetAxisRaw("Horizontal");
        deltaY = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            locomotion.Jump();
        }

    }

    private void FixedUpdate()
    {
        locomotion.Move(deltaX, deltaY);
    }
}
