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

    [Header("SFX")]
    [SerializeField] private string grappleThrowPath = "event:/SFX/Enemies/Crystalline Grapple Shot";
    [SerializeField] private string grappleStickPath = "event:/SFX/Enemies/Crystalline Grapple Stick";
    [SerializeField] private string grappleFailPath = "event:/SFX/Enemies/Crystalline Grapple Fail";
    [SerializeField] private string grapplePullPath = "event:/SFX/Enemies/Crystalline Grapple Pull";

    private FMOD.Studio.EventInstance grappleThrowInstance;
    private FMOD.Studio.EventInstance grappleStickInstance;
    private FMOD.Studio.EventInstance grappleFailInstance;
    private FMOD.Studio.EventInstance grapplePullInstance;

    private bool grapplePullSFXDoOnce = true;

    private Vector2 grapplePoint;
    private Vector2 ropePoint;

    public bool launching { get; private set; } = false; // the organism is launching the grapple, it moves to aiming point
    private bool grappling = false; // the organism is grappling to a surface, the organism moves to the grapple point
    private bool retracting = false; // the organism is retracting the grapple, it goes back to organism

    private GameObject defaultAimPoint; //the aim point used by the enemy

    public delegate void OnGrappleHit();
    public OnGrappleHit onGrappleHit;

    // Start is called before the first frame update
    void Start()
    {
        defaultAimPoint = aimPoint;
        grappleJoint.enabled = false;
        rope.enabled = false;
        Physics2D.queriesStartInColliders = false;

        grappleThrowInstance = FMODUnity.RuntimeManager.CreateInstance(grappleThrowPath);
        grappleStickInstance = FMODUnity.RuntimeManager.CreateInstance(grappleStickPath);
        grappleFailInstance = FMODUnity.RuntimeManager.CreateInstance(grappleFailPath);
        grapplePullInstance = FMODUnity.RuntimeManager.CreateInstance(grapplePullPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (rope.enabled)
        {
            if (launching)
            {
                // Update grapple hook position, lerp from starting point to aim point
                ropePoint = Vector2.Lerp(ropePoint, aimPoint.transform.position, Time.deltaTime * ropeLauchSpeed);
                rope.SetPosition(1, transform.position);
                rope.SetPosition(0, ropePoint);

                // Calculate current length of grapple
                Vector2 direction = ropePoint - (Vector2)transform.position;
                float distance = Vector2.Distance(transform.position, ropePoint);
                if (distance < grappleDistance)
                // grapple hasnt reached maximum distance
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, grappleLayer);
                    if (hit.collider != null)
                    {
                        IActivatableElement activatableElement = hit.collider.GetComponent<IActivatableElement>();
                        if (hit.collider.gameObject.CompareTag("MapElement") && activatableElement == null && !hit.collider.gameObject.GetComponent<FreeMovement>())
                        {
                            KineticReceptor kineticReceptor = hit.collider.gameObject.GetComponent<KineticReceptor>();
                            if (kineticReceptor)
                            // collided object is a receptor
                            {
                                kineticReceptor.Stabbed();
                                onGrappleHit?.Invoke();
                            }
                            else
                            // collided object is tilemap
                            {
                                // activate joint
                                grapplePoint = hit.point;
                                grappleJoint.connectedAnchor = grapplePoint;
                                grappleJoint.enabled = true;

                                launching = false;
                                grappling = true;
                                onGrappleHit?.Invoke();

                                grappleStickInstance.start();
                            }
                        }
                        else
                        {
                            launching = false;
                            retracting = true;

                            grappleFailInstance.start();
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
                // update grapple position 
                rope.SetPosition(1, transform.position);
                rope.SetPosition(0, ropePoint);

                if (grapplePullSFXDoOnce)
                {
                    grapplePullInstance.start();
                    grapplePullSFXDoOnce = false;
                }

                if (Vector2.Distance(transform.position, ropePoint) < 0.7f)
                {
                    DismantleGrapple();
                }
            }
            else if (retracting)
            {

                // move grapple hook back to the organism
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

    public bool LaunchGrapple()
    {
        if (grappling || launching) return false;

        ropePoint = transform.position;
        rope.SetPosition(1, transform.position);
        rope.SetPosition(0, ropePoint);
        rope.enabled = true;
        launching = true;

        grappleThrowInstance.start();
        return true;
    }

    public void DismantleGrapple()
    {
        // disable elements and reset flags
        grappleJoint.enabled = false;
        rope.enabled = false;
        grappling = false;
        launching = false;
        retracting = false;
        grapplePullSFXDoOnce = true;
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

    public void SetAimPoint(GameObject newAimPoint)
    {
        aimPoint = newAimPoint;
    }

    public void ResetAimPoint()
    {
        aimPoint = defaultAimPoint;
    }
}
