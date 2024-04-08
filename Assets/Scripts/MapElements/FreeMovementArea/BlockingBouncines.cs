using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockingBouncines : MonoBehaviour
{
    private bool isOnBlockingBouncines = false;
    private BoxCollider2D boxCollider2D;
    

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isOnBlockingBouncines && GameManager.Instance.GetPlayerController().GetDeltaY() < 0.0f)
        {
            enabled = false;
            boxCollider2D.enabled = false;
            gameObject.SetActive(false);
            isOnBlockingBouncines = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnBlockingBouncines = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnBlockingBouncines = false;
    }
}
