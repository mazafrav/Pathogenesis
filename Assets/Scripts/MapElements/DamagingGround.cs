using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingGround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) //Damage an enemy
        {
            other.GetComponent<DamageControl>().Damage(other);
        }
        else if (other.CompareTag("Player")) //Damage a player
        {
            other.GetComponentInParent<DamageControl>().Damage(other);
        }
    }
}
