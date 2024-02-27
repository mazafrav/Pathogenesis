using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public float followSpeed = 1.5f;
    [SerializeField]
    public float zPos = -10f;
    [SerializeField]
    public float yOffset = 1f;
    [SerializeField]
    public float lookAheadDistance = 5f, lookAheadSpeed = 1f;
    [SerializeField]
    public PlayerController player;

    private float lookOffset = 0f;
    private Vector3 targetPoint = Vector3.zero;

    private void Start()
    {
        targetPoint = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, zPos);
    }
    // Update is called once per frame
    void Update()
    {
        // TODO: hay q cambiar esto un poco para q no dependa de un locomotion especifico (osea player ahora mismo)

        // if (player.rb2D.velocity.x > 0f)
        // {
        //     lookOffset = Mathf.Lerp(lookOffset, lookAheadDistance, lookAheadSpeed * Time.deltaTime);
        // } else if (player.rb2D.velocity.x < 0f)
        // {
        //     lookOffset = -Mathf.Lerp(lookOffset, lookAheadDistance, lookAheadSpeed * Time.deltaTime); ;
        // }

        // targetPoint.x = player.transform.position.x + lookOffset;
        // if (player.groundChecker.isGrounded) 
        // {
        //     targetPoint.y = player.transform.position.y + yOffset;
        // }
        

        // transform.position = Vector3.Slerp(transform.position, targetPoint, followSpeed * Time.deltaTime);
    }
}
