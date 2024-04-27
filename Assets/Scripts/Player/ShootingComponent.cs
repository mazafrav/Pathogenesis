using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingComponent : MonoBehaviour
{
    public bool bisActive = true;

    public void Aim(Vector2 target)
    {
        Debug.Log("Aiming: " + target + " " + bisActive);
        // if(!bisActive) return;
        transform.up = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
    }
}
