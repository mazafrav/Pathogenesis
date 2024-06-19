using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingGround : MonoBehaviour
{
    private DamageControl damageControl;

    private void Start()
    {
        damageControl = GetComponent<DamageControl>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Enemy")) //Damage an enemy
        //{
        //    other.GetComponent<DamageControl>().Damage(other);
        //}
        //else if (other.CompareTag("Player")) //Damage a player
        //{
        //    other.GetComponentInParent<DamageControl>().Damage(other);
        //}

        damageControl.Damage(other);

    }
}
