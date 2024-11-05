using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingGround : MonoBehaviour
{
    private DamageControl damageControl;

    private FMODUnity.StudioEventEmitter emitter;


    private void Start()
    {
        damageControl = GetComponent<DamageControl>();
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
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

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            emitter.Play();
        }

        damageControl.Damage(other);

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        damageControl.Damage(other);
    }
}
