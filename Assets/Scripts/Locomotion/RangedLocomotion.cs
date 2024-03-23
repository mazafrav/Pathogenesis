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

    public override void Attack(float rotation = 0.0f)
    {
        Instantiate(bulletPrefab, shootOrigin.transform.position, Quaternion.Euler(0, 0, rotation + 90));
    }

    public override bool IsAttackReady()
    {
        return false;
    }

    public override void Jump(float deltaX)
    {
        rb2D.velocity = new Vector2(moveSpeed * deltaX, 4);
    }

    public override void Move(float deltaX, float deltaY = 0)
    {
        rb2D.velocity = new Vector2(deltaX * moveSpeed, rb2D.velocity.y);
    }
}
