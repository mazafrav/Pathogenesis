using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected Transform[] wayPoints;
    [SerializeField]
    private ParticleSystem deathEffect;

    public bool IsDead = false;
    [SerializeField]
    private Interactable hostInteractable;

    protected virtual void OnDeath()
    {
        hostInteractable.enabled = true;
        IsDead = true;
        this.enabled = false;
    }

    public void DestroyEnemy()
    {
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }
}
