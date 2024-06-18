using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesDirection : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    private CrystallineLocomotion cristalLocomotion;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        cristalLocomotion = GetComponentInParent<CrystallineLocomotion>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float newRotation;
        switch (cristalLocomotion.directionClimb)
        {
            case AdhesionDirection.N:
                newRotation = 180;
                break;
            case AdhesionDirection.E:
                newRotation = -90;
                break;
            case AdhesionDirection.S:
                newRotation = 0;
                break;
            case AdhesionDirection.W:
                newRotation = 90;
                break;
            default:
                newRotation = 0;
                break;
        }
        //Quaternion newRotation = Quaternion.Euler(0, 0, newRotation);
        spriteRenderer.transform.rotation = Quaternion.LerpUnclamped(spriteRenderer.transform.rotation, Quaternion.Euler(0, 0, newRotation), Time.deltaTime * speed);
    }
}
