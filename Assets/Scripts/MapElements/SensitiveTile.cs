using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SensitiveTile : MonoBehaviour
{
    //ONE WAY TRIGGER
    //Will only trigger if exiting in the same direction that the body entered the collision

    [SerializeField]
    public GameObject[] activatableElements;
    [SerializeField]
    public Animator animator;
    // Start is called before the first frame update
    private Vector2 velocityEnter = Vector2.zero;
    private IActivatableElement[] activatableInterfaces;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip activateClip;

    [SerializeField]
    private ReceptorActivationProjectile activationProjectilePrefab; 
    [SerializeField]
    private float timeToActivate = 1.5f;

    private void Start()
    {
        /*
        for (var index = 0; index < activatableElements.Length; index++)
        {
            if (activatableElements[index].GetComponent<IActivatableElement>() == null) 
            { 
                throw new System.Exception("Object does not implement IActivaatbleElement"); 
            }
            GameObject objectActivable = activatableElements[index];

            activatableInterfaces[index] = objectActivable.GetComponent<IActivatableElement>();
        }
        */

        audioSource = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            velocityEnter = other.gameObject.GetComponentInParent<PlayerLocomotion>().rb2D.velocity.normalized;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Vector2 velocityExit = other.gameObject.GetComponentInParent<PlayerLocomotion>().rb2D.velocity.normalized;
            if (Vector2.Dot(velocityEnter, velocityExit) > 0)
            {
                ActivateActivables();
            }
        }
    }

    public void ActivateActivables()
    {
        animator.Play("SensitiveTileActivated");
        audioSource.PlayOneShot(activateClip);
        for (var index = 0; index < activatableElements.Length; index++)
        {
            ReceptorActivationProjectile activationProjectile = Instantiate(activationProjectilePrefab, transform.position, Quaternion.identity) as ReceptorActivationProjectile;
            activationProjectile.Initialize(activatableElements[index], timeToActivate);
            StartCoroutine(activateBlock(activatableElements[index]));
        }
    }

    private IEnumerator activateBlock(GameObject target)
    {
        yield return new WaitForSeconds(timeToActivate);
        target.GetComponent<IActivatableElement>()?.Activate();
    }

}
