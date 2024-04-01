using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySource : MonoBehaviour
{
    // Start is called before the first frame update
    private LightRay ray;
    void Start()
    {
        GameObject oRay = new GameObject("Light Ray");
        oRay.transform.position = transform.position;
        oRay.transform.rotation = transform.rotation;
        ray = oRay.AddComponent<LightRay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
