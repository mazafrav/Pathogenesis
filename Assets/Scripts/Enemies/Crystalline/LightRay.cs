using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        Debug.DrawLine(transform.position, transform.position + transform.up * 20, Color.red);
        if(!hit) return;
        Debug.Log(hit.collider.gameObject.name);
        Vector2 reflect = Vector2.Reflect(transform.up, hit.normal);
        Debug.DrawLine(hit.point, hit.point + reflect * 20, Color.green);
    }
}
