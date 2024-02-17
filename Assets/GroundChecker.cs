using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool CanJump { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CanJump = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CanJump = false;
    }
}
