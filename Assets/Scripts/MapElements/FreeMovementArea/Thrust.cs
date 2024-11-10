using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class Thrust : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject thrustBlocking;
    [SerializeField] private bool showArrowGizmo = true;
    [SerializeField] private ParticleSystem thurstVFX;
    [SerializeField] private float thrust = 20.0f;
    [Header("Vertical Thrust")]
    [SerializeField] private bool applyVerticalThrust = true;
    [Header("Horizontal Thrust")]
    [SerializeField] private bool applyHorizontalThrust = false;
    [SerializeField] private float disabledControlsTime = 1.0f;

    private FMODUnity.StudioEventEmitter emitter;

    private bool hasDisabledControls = false;
    private float currentTime = 0.0f;

    private Animator animator;
    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        currentTime = disabledControlsTime;
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    void Update()
    {
        if(applyHorizontalThrust)
        {
            if (hasDisabledControls)
            {
                currentTime -= Time.deltaTime;
            }
            if (currentTime <= 0.0f)
            {
                currentTime = disabledControlsTime;
                hasDisabledControls = false;
                PlayerController playerController = GameManager.Instance.GetPlayerController();
                //PlayerLocomotion playerLocomotion = GameManager.Instance.GetPlayerLocomotion();
                //playerLocomotion.DisableFreeMovement();
                playerController.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        if(applyHorizontalThrust)
        {
            PlayerController playerController = collision.GetComponentInParent<PlayerController>();

            Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
            if (dir.x > 0.0f || dir.x < 0.0f)
            {
                playerController.enabled = false;
                hasDisabledControls = true;
            }
            //playerLocomotion.EnableFreeMovement();
            //collision.GetComponent<Rigidbody2D>().velocity = transform.right * thrust;
            collision.GetComponent<Rigidbody2D>().AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }
        else if(applyVerticalThrust)
        {
            ActivateThrustBlocking(false);
           
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>(); 

            if(playerLocomotion && rb)
            {
                playerLocomotion.DisableFreeMovement();
                rb.AddForce(new Vector3(0, 1) * thrust, ForceMode2D.Impulse);             
            }
        }

        animator.Play("FMAT_Thurst");
        emitter.Play();
        if (thurstVFX) { thurstVFX.Play(); };
    }

    private void OnTriggerExit2D(Collider2D collision)
    {     
        StartCoroutine(EnableThrust());
    }

    public void ActivateThrustBlocking(bool isActive)
    {
        thrustBlocking.SetActive(isActive);
        thrustBlocking.GetComponent<BoxCollider2D>().enabled = isActive;           
        thrustBlocking.GetComponent<ThrustBlocking>().enabled = isActive;
    }

    private IEnumerator EnableThrust()
    {
        yield return new WaitForSeconds(0.2f);
        ActivateThrustBlocking(true);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showArrowGizmo)
        {
            Gizmos.color = Color.red;

            float lineLenght = 2.0f;
            float arrowLineLenght = 0.3f;

            if(applyHorizontalThrust)
            {
                Gizmos.DrawLine(transform.position, transform.position + transform.up * lineLenght);

                Vector3 startPos = transform.position + transform.up * lineLenght;

                if (transform.rotation.eulerAngles.z == 90.0f)
                {
                    Gizmos.DrawLine(startPos, startPos + new Vector3(1, 1) * arrowLineLenght);
                    Gizmos.DrawLine(startPos, startPos + new Vector3(1, -1) * arrowLineLenght);
                }
                else
                {
                    Gizmos.DrawLine(startPos, startPos + new Vector3(-1, 1) * arrowLineLenght);
                    Gizmos.DrawLine(startPos, startPos + new Vector3(-1, -1) * arrowLineLenght);
                }
            }
            else if (applyVerticalThrust)
            {
                Gizmos.DrawLine(transform.position, transform.position + transform.up * lineLenght);

                Vector3 startPos = transform.position + transform.up * lineLenght;
                Gizmos.DrawLine(startPos, startPos + new Vector3(1, -1) * arrowLineLenght);
                Gizmos.DrawLine(startPos, startPos + new Vector3(-1, -1) * arrowLineLenght);
            }    
        }
    }
#endif
}
