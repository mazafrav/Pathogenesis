using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleDistance;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private GameObject aimPoint;
    [SerializeField] private SliderJoint2D grappleJoint;
    [SerializeField] private LineRenderer rope;

    private Vector3 grapplePoint;


    private bool isGrappling = false;

    // Start is called before the first frame update
    void Start()
    {
        grappleJoint.enabled = false;
        rope.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rope.enabled)
        {
            rope.SetPosition(1, transform.position);
        }
    }

    public void LaunchGrapple()
    {
        isGrappling = true;
        Vector2 direction = aimPoint.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, grappleDistance, grappleLayer);
        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            grapplePoint.z = 0f;
            grappleJoint.connectedAnchor = grapplePoint;
            grappleJoint.enabled = true;
            //grappleJoint.distance = 0.2f;

            rope.SetPosition(0, grapplePoint);
            rope.SetPosition(1, transform.position);
            rope.enabled = true;
        }
    }

    public void DismantleGrapple()
    {
        grappleJoint.enabled=false;
        rope.enabled=false;
    }
}
