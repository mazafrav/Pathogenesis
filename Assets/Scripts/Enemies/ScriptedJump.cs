using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HostLocomotion hostLocomotion = collision.gameObject.GetComponent<HostLocomotion>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        //The enemy dont have to be possessed
        if (hostLocomotion && !enemy.IsPossesed && !collision.gameObject.CompareTag("Player"))
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
