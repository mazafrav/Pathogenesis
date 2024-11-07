using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockingBouncines : MonoBehaviour
{
    [Header("Condition to disable bloking bouncines")]
    [SerializeField] private bool moveDown = true;
    [SerializeField] private bool moveRight = false;
    [SerializeField] private bool moveLeft = false;

    private bool isOnBlockingBouncines = false;
    private BoxCollider2D boxCollider2D;
    

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isOnBlockingBouncines && CheckBlockingBouncinesDisabledCondition())
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

    private bool CheckBlockingBouncinesDisabledCondition()
    {
        if (moveDown)
        {
            return GameManager.Instance.GetPlayerController().GetDeltaY() < 0.0f;
        }
        else if (moveRight)
        {
            return GameManager.Instance.GetPlayerController().GetDeltaX() > 0.0f;
        }
        else if (moveLeft)
        {
            return GameManager.Instance.GetPlayerController().GetDeltaX() < 0.0f;
        }
        else
        {
            return GameManager.Instance.GetPlayerController().GetDeltaY() < 0.0f;
        }
    }
}
