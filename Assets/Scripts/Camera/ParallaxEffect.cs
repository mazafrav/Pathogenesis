using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] public float parallaxAmount;
    [SerializeField] public Camera mainCamera;

    private float startingPos;
    private float lengthOfSprite;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position.x;
        lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (mainCamera.transform.position.x * (1 - parallaxAmount));
        float dist = (mainCamera.transform.position.x * parallaxAmount);

        transform.position = new Vector3(startingPos + dist, transform.position.y, transform.position.z);

        if (temp > startingPos + lengthOfSprite / 2) startingPos += lengthOfSprite;
        else if (temp < startingPos - lengthOfSprite / 2) startingPos -= lengthOfSprite;
    }
}
