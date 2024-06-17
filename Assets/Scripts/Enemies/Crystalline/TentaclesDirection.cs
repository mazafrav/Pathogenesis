using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesDirection : MonoBehaviour
{
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
        switch (cristalLocomotion.directionClimb)
        {
            case AdhesionDirection.N:
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case AdhesionDirection.E:
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case AdhesionDirection.S:
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case AdhesionDirection.W:
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            default:
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }
}
