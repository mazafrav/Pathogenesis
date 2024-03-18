using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsDead = false;
    [SerializeField]
    private Interactable hostInteractable;

    protected virtual void OnDeath()
    {
        hostInteractable.enabled = true;
        IsDead = true;
        this.enabled = false;
    }
}
