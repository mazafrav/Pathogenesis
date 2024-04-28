using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HostLocomotion hostLocomotion = collision.gameObject.GetComponent<HostLocomotion>();
        if (hostLocomotion && !collision.gameObject.CompareTag("Player"))
        {
            if ((transform.position.x - collision.transform.position.x) < 0)
            {
                hostLocomotion.Jump(-1.0f);
            }
            else if ((transform.position.x - collision.transform.position.x) > 0)
            {
                hostLocomotion.Jump(1.0f);
            }          
        }
    }
}
