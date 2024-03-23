using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingComponent : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    private GameObject playerBody;

    public bool bisActive = true;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerBody = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bisActive)
        {
            Aim();
        }
        
    }

    public void Aim()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rot = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;

        playerBody.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
