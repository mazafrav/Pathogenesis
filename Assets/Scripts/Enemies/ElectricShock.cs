using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);

        Health health = collision.gameObject.GetComponent<Health>();           
        if (health)
        {
            health.DestroyOwner();
        }       
    }
}
