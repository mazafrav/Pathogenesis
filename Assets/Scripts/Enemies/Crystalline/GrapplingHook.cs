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
    [SerializeField] private float ropeLauchSpeed = 5f;
    [SerializeField] private LineRenderer rope;

    private Vector2 grapplePoint;
    private Vector2 ropePoint;
    private bool grappled = false;

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
            if (!grappled)
            {
                ropePoint = Vector2.Lerp(ropePoint, aimPoint.transform.position, Time.deltaTime * ropeLauchSpeed);
                rope.SetPosition(1, transform.position);
                rope.SetPosition(0, ropePoint);

                Vector2 direction = ropePoint - (Vector2)transform.position;
                float distance = Vector2.Distance(transform.position, ropePoint);
                if (distance < grappleDistance)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, grappleLayer);
                    if (hit.collider != null)
                    {
                        grapplePoint = hit.point;
                        grappleJoint.connectedAnchor = grapplePoint;
                        grappleJoint.enabled = true;

                        grappled = true;
                    }
                } 
                else
                {
                    DismantleGrapple();
                }
            } 
            else
            {
                rope.SetPosition(1, transform.position);
                rope.SetPosition(0, ropePoint);
            }
        }
    }

    public void LaunchGrapple()
    {
        grappled = false;
        ropePoint = transform.position;
        rope.SetPosition(1, transform.position);
        rope.SetPosition(0, ropePoint);
        rope.enabled = true;
    }

    public void DismantleGrapple()
    {
        grappleJoint.enabled = false;
        rope.enabled = false;
        grappled = false;
    }


}
