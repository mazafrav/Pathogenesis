using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeEndRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponentInParent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.enabled)
        {
            spriteRenderer.enabled = true;
            transform.position = lineRenderer.GetPosition(0);
        } 
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
