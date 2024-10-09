using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustTrigger : MonoBehaviour
{
    public bool CanThrust { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(CanThrust);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CanThrust = true;
        Debug.Log("enter");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //CanThrust = false;
        Debug.Log("exit");
        Invoke("DeactivateCanThrust", 0.5f);

    }

    void DeactivateCanThrust()
    {
        CanThrust = false;
    }

    //private void ApplyThrust(PlayerController playerController, Collider2D collision)
    //{

    //    Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
    //    if (dir.x > 0.0f || dir.x < 0.0f)
    //    {
    //        playerController.enabled = false;
    //        hasDisabledControls = true;
    //    }
    //    //collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * 25, ForceMode2D.Impulse);

    //    collision.GetComponent<Rigidbody2D>().velocity = new Vector2(30, 0);


    //    //else if (applyThrustOverTheFloor)
    //    //{
    //    //    Vector2 dir = new Vector2(playerController.GetDeltaX(), playerController.GetDeltaY());
    //    //    if (dir.x > 0.0f || dir.x < 0.0f)
    //    //    {
    //    //        playerController.enabled = false;
    //    //        hasDisabledControls = true;
    //    //    }
    //    //    collision.GetComponent<Rigidbody2D>().AddForce(dir * thrust, ForceMode2D.Impulse);
    //    //}
    //}
}
