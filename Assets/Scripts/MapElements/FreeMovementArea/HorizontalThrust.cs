using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalThrust : MonoBehaviour
{
    [SerializeField] private float thrust = 30.0f;
    [SerializeField] private float time = 1.0f;
    [SerializeField] private bool showArrowGizmo = true;
    private bool hasDisabledControls = false;
    private float currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasDisabledControls)
        {
            currentTime -= Time.deltaTime;
        }
        if (currentTime <= 0.0f)
        {
            currentTime = time;
            hasDisabledControls = false;
            PlayerController playerController = GameManager.Instance.GetPlayerController();
            //PlayerLocomotion playerLocomotion = GameManager.Instance.GetPlayerLocomotion();
            //playerLocomotion.DisableFreeMovement();
            playerController.enabled = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLocomotion playerLocomotion = collision.GetComponentInParent<PlayerLocomotion>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>(); 

        Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
        if (dir.x > 0.0f || dir.x < 0.0f)
        {
            playerController.enabled = false;
            hasDisabledControls = true;
        }
        //playerLocomotion.EnableFreeMovement();
        //collision.GetComponent<Rigidbody2D>().velocity = transform.right * thrust;
        collision.GetComponent<Rigidbody2D>().AddForce(transform.right * thrust, ForceMode2D.Impulse);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showArrowGizmo)
        {
            Gizmos.color = Color.red;

            float lineLenght = 2.0f;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * lineLenght);

            float arrowLineLenght = 0.3f;
            Vector3 startPos = transform.position + transform.right * lineLenght;

            if(transform.rotation.eulerAngles.z == 180.0f)
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
    }
#endif
}
