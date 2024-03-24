using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HostLocomotion : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    protected float moveSpeed = 5.0f;

    [Header("Jump")]
    [SerializeField]
    protected float jumpHeight = 2;
    [SerializeField]
    protected float jumpDistance = 3;
    public Rigidbody2D rb2D {protected set; get;} = null;

    public abstract void Jump(float deltaX);
    public abstract void Move(float deltaX, float deltaY);
    public abstract void Attack();
    public abstract bool IsAttackReady();
}
