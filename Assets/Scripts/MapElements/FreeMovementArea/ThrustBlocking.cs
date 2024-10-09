using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustBlocking : MonoBehaviour
{
    [SerializeField] private float blockingThrust = 1.5f;

    private bool isOnThrustBlocking = false;
    private BoxCollider2D boxCollider2D;


    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isOnThrustBlocking && GameManager.Instance.GetPlayerController().GetDeltaY() < 0.0f)
        {      
            GameManager.Instance.GetPlayerController().GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0, 1) * blockingThrust, ForceMode2D.Impulse);         
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnThrustBlocking = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnThrustBlocking = false;
    }


}
