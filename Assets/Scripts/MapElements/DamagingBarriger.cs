using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingBarriger : MonoBehaviour
{
    private DamageControl damageControl;

    private FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        damageControl= GetComponent<DamageControl>();
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            emitter.Play();
            damageControl.Damage(other);
        }

    }  
}
