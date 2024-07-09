using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] public float parallaxAmount;
    [SerializeField] public Camera mainCamera;

    private float startingPosX;
    private float startingPosY;
    private float lengthOfSpriteX;
    private float lengthOfSpriteY;
    // Start is called before the first frame update
    void Start()
    {
        startingPosX = transform.position.x;
        startingPosY = transform.position.y;

        lengthOfSpriteX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthOfSpriteY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = (mainCamera.transform.position.x * (1 - parallaxAmount));
        float distX = (mainCamera.transform.position.x * parallaxAmount);

        float tempY = (mainCamera.transform.position.y * (1 - parallaxAmount));
        float distY = (mainCamera.transform.position.y * parallaxAmount);

        transform.position = new Vector3(startingPosX + distX, startingPosY + distY, transform.position.z);

        if (tempX > startingPosX + lengthOfSpriteX / 2) startingPosX += lengthOfSpriteX;
        else if (tempX < startingPosX - lengthOfSpriteX / 2) startingPosX -= lengthOfSpriteX;

        if (tempY > startingPosY + lengthOfSpriteY / 2) startingPosY += lengthOfSpriteY;
        else if (tempY < startingPosY - lengthOfSpriteY / 2) startingPosY -= lengthOfSpriteY;
    }
}
