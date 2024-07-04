using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingBarriger : MonoBehaviour
{
    //private Vector2 velocityEnter = Vector2.zero;

    private DamageControl damageControl;

    [SerializeField]
    private AudioClip damageClip;
    private AudioSource audioSource;

    private void Start()
    {
        damageControl= GetComponent<DamageControl>();
        audioSource = GetComponent<AudioSource>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Get direcion of movement of playr entering the field
        //if (other.tag == "Player")
        //{
        //    velocityEnter = other.gameObject.GetComponentInParent<PlayerLocomotion>().rb2D.velocity.normalized;
        //}
        //else if (other.tag == "Enemy")
        //{
        //    velocityEnter = other.gameObject.GetComponent<Rigidbody2D>().velocity;
        //}

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(damageClip);
        }

        damageControl.Damage(other);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //Apply damage is player leaving the field matains the same direction
        //if (other.tag == "Player")
        //{
        //    Vector2 velocityExit = other.gameObject.GetComponentInParent<PlayerLocomotion>().rb2D.velocity.normalized;
        //    if (Vector2.Dot(velocityEnter, velocityExit) > 0)
        //    {
        //        other.GetComponentInParent<DamageControl>().Damage(other);
        //    }
        //} 
        //else if (other.tag == "Enemy")
        //{
        //    Vector2 velocityExit = other.gameObject.GetComponent<Rigidbody2D>().velocity = velocityEnter;
        //    if (Vector2.Dot(velocityEnter, velocityExit) > 0)
        //    {
        //        other.GetComponent<DamageControl>().Damage(other);
        //    }
        //}
    }
}
