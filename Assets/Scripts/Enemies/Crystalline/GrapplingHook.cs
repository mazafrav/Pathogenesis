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

    private bool launching = false; // the organism is launching the grapple
    private bool grappling = false; // the organism is grappling to a surface
    private bool retracting = false; // the organism is retracting the grapple

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
            if ( launching )
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
                        if (hit.collider.gameObject.CompareTag("Grappable"))
                        {
                            grapplePoint = hit.point;
                            grappleJoint.connectedAnchor = grapplePoint;
                            grappleJoint.enabled = true;

                            launching = false;
                            grappling = true;
                        }
                        else
                        {
                            launching = false;
                            retracting = true;
                        }
                    }
                } 
                else
                {
                    launching = false;
                    retracting = true;
                }
            } 
            else if (grappling)
            {
                rope.SetPosition(1, transform.position);
                rope.SetPosition(0, ropePoint);
            }
            else if ( retracting )
            {
                ropePoint = Vector2.Lerp(ropePoint, transform.position, Time.deltaTime * ropeLauchSpeed * 2);
                rope.SetPosition(1, transform.position);
                rope.SetPosition(0, ropePoint);
                if (Vector2.Distance(transform.position, ropePoint) < 0.5f)
                {
                    DismantleGrapple();
                }
            }
        }
    }

    public void LaunchGrapple()
    {
        if (!grappling && !launching)
        {
            ropePoint = transform.position;
            rope.SetPosition(1, transform.position);
            rope.SetPosition(0, ropePoint);
            rope.enabled = true;
            launching = true;
        }
    }

    public void DismantleGrapple()
    {
        grappleJoint.enabled = false;
        rope.enabled = false;
        grappling = false;
        launching = false;
        retracting = false;
    }

    public void CancelGrapple()
    {
        if (launching)
        {
            launching = false;
            retracting = true;
        }
        else if (grappling)
        {
            DismantleGrapple();
        }
    }

}
