using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedLocomotion : HostLocomotion
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shootOrigin;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponentInParent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        Instantiate(bulletPrefab, shootOrigin.transform.position, Quaternion.identity);
    }

    public override bool IsAttackReady()
    {
        return false;
    }

    public override void Jump(float deltaX)
    {

    }

    public override void Move(float deltaX, float deltaY = 0)
    {
        rb2D.velocity = new Vector2(deltaX * moveSpeed, rb2D.velocity.y);
    }
}
