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
    [SerializeField]
    protected float coyoteTime = 0.1f;
    protected float coyoteTimeCounter;
    [SerializeField]
    protected float jumpBufferTime = 0.1f;
    protected float jumpBufferCounter;

    public Rigidbody2D rb2D { protected set; get; } = null;

    public abstract void Jump(float deltaX);
    public void JumpButtonUp() { coyoteTimeCounter = 0; }
    public abstract void Move(float deltaX, float deltaY);
    public abstract void Attack();
    public abstract bool IsAttackReady();
}
