using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingGround : MonoBehaviour
{
    private DamageControl damageControl;
    [SerializeField]
    private AudioClip damageClip;
    private AudioSource audioSource;

    private void Start()
    {
        damageControl = GetComponent<DamageControl>();
        audioSource = GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(damageClip);
        }

        damageControl.Damage(other);

    }
}
